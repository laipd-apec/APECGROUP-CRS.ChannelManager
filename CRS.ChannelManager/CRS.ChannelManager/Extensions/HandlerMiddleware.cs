using CRS.ChannelManager.Domain;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.HanderException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;

namespace CRS.ChannelManager.Extensions
{
    public class HandlerMiddleware : BaseExceptionMiddleware
    {
        private IHttpContextAccessor _contextAccessor;
        public HandlerMiddleware(RequestDelegate next
            , ILogger<HandlerMiddleware> logger
            , IHttpContextAccessor contextAccessor) : base(next, logger)
        {
            _contextAccessor = contextAccessor;
        }

        public virtual async Task Invoke(HttpContext context)
        {
            bool isContinue = true;
            CRSChannelManagerContext dbContext = null;
            try
            {
                var endpoint = context.GetEndpoint();
                var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (actionDescriptor != null)
                {
                    bool allowAnonymous = actionDescriptor.EndpointMetadata.Any(t => t is AllowAnonymousAttribute);
                    if (!allowAnonymous)
                    {
                        //var userLogin = CommonUtils.GetCurrentUsername(_contextAccessor);
                        //if (!string.IsNullOrEmpty(userLogin))
                        //{
                        //    string tokenRequest = CommonUtils.GetCurrentToken(_contextAccessor);
                        //    string? cacheTokenUser = string.Empty;
                        //    _cache.TryGetValue(userLogin.ToLower(), out cacheTokenUser);
                        //    if (string.IsNullOrEmpty(cacheTokenUser) || (!string.IsNullOrEmpty(tokenRequest) && !tokenRequest.Equals(cacheTokenUser)))
                        //    {
                        //        isContinue = false;
                        //        List<ErrorDto> lstError = new List<ErrorDto>();
                        //        lstError.Add(new ErrorDto((int)HttpStatusCode.Unauthorized, "Token Unauthorized"));
                        //        context.Response.ContentType = ContentTypeDefault;
                        //        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        //        var result = new ResultBaseDto<string>(false, lstError.Distinct().ToList(), null);
                        //        base.responseJson = JsonConvert.SerializeObject(result);
                        //        await context.Response.WriteAsync(base.responseJson);
                        //        //throw new Exception("Token Unauthorized");
                        //    }
                        //}
                    }
                }
                if (isContinue)
                {
                    await base.Invoke(context);
                }
            }
            catch (Exception exceptionObj)
            {
                List<ErrorDto> lstError = new List<ErrorDto>();
                context.Response.ContentType = ContentTypeDefault;
                lstError.Add(new ErrorDto((int)HttpStatusCode.InternalServerError, exceptionObj.Message));
                var result = new ResultBaseDto<string>(false, lstError.Distinct().ToList(), null);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogError("HandlerMiddleware Exception: {@Message}", JsonConvert.SerializeObject(result));
                base.responseJson = JsonConvert.SerializeObject(result);
                await context.Response.WriteAsync(base.responseJson);
            }
            finally
            {
                //if (context.Request.Headers.ContentType.Contains(ContentTypeDefault)
                //    && context.Response.Headers.ContentType.ToString().Contains(ContentTypeDefault))
                //{
                //    dbContext = (ASMServiceContext)context.RequestServices.GetService<IDbContext>();
                //    if (dbContext != null)
                //    {
                //        dbContext.RequestResponseLogEntity.Add(new Domain.Entities.RequestResponseLogEntity()
                //        {
                //            Url = base.path,
                //            Method = base.method.ToUpper(),
                //            Header = base.header,
                //            Request = base.requestJson,
                //            Response = base.responseJson,
                //        });
                //        dbContext.SaveChanges();
                //    }
                //}
            }
        }
    }
}
