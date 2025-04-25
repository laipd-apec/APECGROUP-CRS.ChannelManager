using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;
using CRS.ChannelManager.Library.Base;
using System.Text.Json.Nodes;
using Nest;
using Microsoft.Extensions.Logging;

namespace CRS.ChannelManager.Library.HanderException
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IElasticClient _elasticClient;

        private ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var originBody = context.Response.Body;
            string nullValue = null;
            using (var responseBody = new MemoryStream())
            {
                try
                {
                    // Process inner middlewares and return result.
                    if (context.Request.ContentType == null || context.Request.ContentType.IndexOf("json") > 0)
                    {
                        context.Response.Body = responseBody;
                        await _next(context);
                        responseBody.Seek(0, SeekOrigin.Begin);
                        using (var streamReader = new StreamReader(responseBody))
                        {
                            // Get action result come from mvc pipeline
                            var strActionResult = streamReader.ReadToEnd();
                            try
                            {
                                var tmpObj = JsonValue.Parse(strActionResult);
                                var objActionResult = JsonConvert.DeserializeObject(strActionResult);
                                var objectValue = JsonConvert.DeserializeObject<ReturnStatusDto>(strActionResult);
                                if (context.Response.ContentType.IndexOf("json") <= 0)
                                {
                                    context.Response.Body = originBody;
                                    await context.Response.WriteAsync(strActionResult);
                                }
                                else
                                {
                                    context.Response.Body = originBody;
                                    if (objectValue != null && objectValue.status != 0)
                                    {
                                        var Errors = new ArrayList();
                                        JObject jObject = JObject.Parse(objectValue?.errors?.ToString());

                                        if (jObject.Children().Any())
                                        {
                                            foreach (var item in jObject.Children())
                                            {
                                                Type typeItem = item.GetType();
                                                PropertyInfo[] propsItem = typeItem.GetProperties();

                                                var name = propsItem.Where(t => t.Name.ToLower().Equals("value")).FirstOrDefault().GetValue(item);
                                                if (name != null)
                                                {
                                                    var array = JArray.Parse(name.ToString());
                                                    foreach (var valueArray in array)
                                                    {
                                                        var errorObject = new
                                                        {
                                                            id = 0,
                                                            code = objectValue?.status != 0 ? objectValue?.status : (int)HttpStatusCode.OK,
                                                            message = valueArray.ToString()
                                                        };
                                                        Errors.Add(errorObject);
                                                    }
                                                }
                                            }
                                        }
                                        objActionResult = new
                                        {
                                            data = nullValue,
                                            pagination = nullValue,
                                            isSuccess = false,
                                            errors = Errors//objectValue.errors
                                        };
                                    }
                                    // Create uniuqe shape for all responses.
                                    var responseModel = objActionResult;

                                    // Set all response code to 200 and keep actual status code inside wrapped object.
                                    if (objectValue == null)
                                    {
                                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                                    }
                                    else
                                    {
                                        context.Response.StatusCode = objectValue.status != 0 ? objectValue.status : (int)HttpStatusCode.OK;
                                    }
                                    await context.Response.WriteAsync(JsonConvert.SerializeObject(responseModel));
                                }

                            }
                            catch (Exception fex)
                            {
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(strActionResult);
                            }
                        }
                    }
                    else
                    {
                        await _next(context);
                    }
                }
                catch (DomainExceptionBase error)
                {
                    //var response = context.Response;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Body = originBody;

                    if (error.ErrorDtos != null)
                    {
                        var result = JsonConvert.SerializeObject(new
                        {
                            data = nullValue,
                            pagination = nullValue,
                            isSuccess = false,
                            errors = error.ErrorDtos.Select(t => new { id = t.Id, code = t.Code, message = t.Message })
                        });
                        await context.Response.WriteAsync(result);
                    }
                    else
                    {
                        var result = JsonConvert.SerializeObject(new
                        {
                            data = nullValue,
                            pagination = nullValue,
                            isSuccess = false,
                            errors = new List<dynamic>
                            {
                              new { id = nullValue, code = 500,message = error.Message }
                            }
                        });
                        await context.Response.WriteAsync(result);
                    }
                    _logger.LogError("Exception: {@Message}", JsonConvert.SerializeObject(error.ErrorDtos));
                }
            }
        }
    }
}
