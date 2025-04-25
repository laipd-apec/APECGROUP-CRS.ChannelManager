using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseDto;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Nest;
using MediatR;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;

namespace CRS.ChannelManager.Library.HanderException
{
    public class BaseExceptionMiddleware
    {
        protected readonly string ContentTypeDefault = "application/json";
        protected readonly RequestDelegate _next;
        protected ILogger<BaseExceptionMiddleware> _logger;
        protected Stream originalBodyStream;
        protected string requestJson = string.Empty;
        protected string responseJson = string.Empty;
        protected string path = string.Empty;
        protected string header = string.Empty;
        protected string method = string.Empty;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public BaseExceptionMiddleware(RequestDelegate next, ILogger<BaseExceptionMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        protected async Task Invoke(HttpContext context)
        {
            List<ErrorDto> lstError = new List<ErrorDto>();
            await _semaphore.WaitAsync();
            try
            {
                var req = context.Request;
                path = context.Request.Path;
                method = context.Request.Method;
                header = JsonConvert.SerializeObject(context.Request.Headers);
                originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    if (req != null)
                    {
                        req.EnableBuffering();
                        using (var readerRequest = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                        {
                            requestJson = await readerRequest.ReadToEndAsync();
                        }
                        if (string.IsNullOrEmpty(context.Request.ContentType))
                        {
                            context.Request.ContentType = ContentTypeDefault;
                        }
                        req.Body.Position = 0;
                    }
                    context.Response.Body = responseBody;
                    await _next(context);
                    responseBody.Seek(0, SeekOrigin.Begin);
                    if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
                    {
                        using (var streamReader = new StreamReader(responseBody))
                        {
                            var strActionResult = streamReader.ReadToEnd();
                            var dataErrors = JsonConvert.DeserializeObject<BaseDto.ModelBadRequestDto>(strActionResult);

                            if (dataErrors != null && dataErrors.Errors != null)
                            {
                                JObject jObject = JObject.Parse(dataErrors?.Errors?.ToString());

                                if (jObject.Children().Any())
                                {
                                    foreach (JToken item in jObject.Children())
                                    {
                                        string name = ((JProperty)item).Name;
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            if (item.Children().Any())
                                            {
                                                foreach (JToken valueArray in item.Children())
                                                {
                                                    if (valueArray != null)
                                                    {
                                                        List<string> err = JsonConvert.DeserializeObject<List<string>>(valueArray.ToString());
                                                        string strErr = $"{string.Join(",", err)}";
                                                        if (!strErr.Contains(name))
                                                        {
                                                            strErr = $"{name} {string.Join(",", err)}";
                                                        }
                                                        lstError.Add(new ErrorDto((int)HttpStatusCode.BadRequest, strErr));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            var result = new ResultBaseDto<string>(false, lstError.Distinct().ToList(), null);
                            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
                            responseJson = JsonConvert.SerializeObject(result);
                            using (var output = new MemoryStream(buffer))
                            {
                                await output.CopyToAsync(originalBodyStream);
                            }
                        }
                    }
                    else
                    {
                        responseBody.Position = 0;
                        responseJson = await new StreamReader(responseBody).ReadToEndAsync();
                        responseBody.Position = 0;
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                    //context.Response.OnStarting(state => { return Task.CompletedTask; }, context);
                    context.Response.Body = originalBodyStream;
                }
            }
            catch (DomainExceptionBase ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        protected async Task HandleExceptionAsync(HttpContext context, DomainExceptionBase exception)
        {
            string data = null;
            List<ErrorDto> lstError = new List<ErrorDto>();
            context.Response.ContentType = ContentTypeDefault;
            if (exception is DomainExceptionBase)
            {
                lstError.AddRange(exception.ErrorDtos);
                var result = new ResultBaseDto<string>(false, lstError.Distinct().ToList(), null);
                data = JsonConvert.SerializeObject(result);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                string messageExe = exception?.InnerException?.Message ?? exception.Message;
                lstError.Add(new ErrorDto((int)HttpStatusCode.BadRequest, messageExe));
                var result = new ResultBaseDto<string>(false, lstError.Distinct().ToList(), null);
                data = JsonConvert.SerializeObject(result);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            responseJson = data;
            _logger.LogError("Exception: {@Message}", data);
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            using (var output = new MemoryStream(buffer))
            {
                await output.CopyToAsync(originalBodyStream);
            }
            context.Response.Body = originalBodyStream;
            //await context.Response.WriteAsync(data);
        }

        protected async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            List<ErrorDto> lstError = new List<ErrorDto>();
            context.Response.ContentType = ContentTypeDefault;
            string messageExe = exception?.InnerException?.Message ?? exception.Message;
            lstError.Add(new ErrorDto((int)HttpStatusCode.InternalServerError, messageExe));
            var result = new ResultBaseDto<string>(false, lstError.Distinct().ToList(), null);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            _logger.LogError("Exception: {@Message}", JsonConvert.SerializeObject(result));
            responseJson = JsonConvert.SerializeObject(result);
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            using (var output = new MemoryStream(buffer))
            {
                await output.CopyToAsync(originalBodyStream);
            }
            context.Response.Body = originalBodyStream;
            //await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

    }
}
