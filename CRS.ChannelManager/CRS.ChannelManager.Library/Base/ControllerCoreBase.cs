using Microsoft.AspNetCore.Mvc;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseInterface;
using CRS.ChannelManager.Library.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using static CRS.ChannelManager.Library.Base.ExtensionAttributes;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Base
{
    [CheckActionAPI(isCheck: false)]
    public class ControllerCoreBase : ControllerBase
    {
        protected virtual IActionResult Result<T>(T model)
        {
            return Result(model, true);
        }

        protected virtual IActionResult Result<T>(T model, Boolean isSuccess)
        {

            if (model == null)
                return Ok(new ResultBaseDto<T>(isSuccess, "Record not found"));

            // Record found
            return Ok(new ResultBaseDto<T>(isSuccess, "", model));
        }

        protected virtual IActionResult Result<T>(List<ErrorDto> model, Boolean isSuccess)
        {

            return Ok(new ResultBaseDto<T>(isSuccess, model));
            // Record found
        }

        protected virtual IActionResult Result<T>(List<ErrorDto> model, Boolean isSuccess, string[] errorMessage)
        {

            return Ok(new ResultBaseDto<T>(isSuccess, model));
            // Record found
        }

        protected virtual IActionResult Result<T>(PagedResultBaseDto<List<T>> model)
        {
            if (model == null)
                return Ok(new ResultBaseDto<T>(true, "Record not found"));

            // Record found
            return Ok(new ResultBaseDto<List<T>>(true, null, model.Result, model.Pagination));
        }

        protected virtual IActionResult BadResult<T>(string message, T model)
        {
            return BadRequest(new ResultBaseDto<T>(false, message, model));
        }

        protected virtual IActionResult BadResult(DomainExceptionBase ex)
        {
            return BadRequest(new ResultBaseDto<string>(false, ex.Message, ex.ErrorDtos, ex.ToString()));
        }
    }

    public class ExtensionAttributes
    {
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
        public class CheckActionAPIAttribute : ActionFilterAttribute
        {
            private bool _isCheck { get; set; }
            private string _permissionConfig = "PermissionConfig";

            public CheckActionAPIAttribute(bool isCheck = false)
            {
                _isCheck = isCheck;
                if (_isCheck)
                {

                }
            }

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                base.OnActionExecuting(context);
            }

            public override void OnActionExecuted(ActionExecutedContext context)
            {
                //CheckPermission(context);
                base.OnActionExecuted(context);
            }

            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                CheckPermission(context);
                await base.OnActionExecutionAsync(context, next);
            }

            private async void CheckPermission(FilterContext context)
            {
                try
                {
                    if (_isCheck)
                    {
                        var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
                        string domainCheck = string.Empty;
                        string urlCheck = string.Empty;
                        string access_token = string.Empty;
                        string pathAPI = context.HttpContext.Request.Path;
                        if (config != null)
                        {
                            access_token = await context.HttpContext.GetTokenAsync("access_token");
                            bool isAuth = false;
                            if (!isAuth)
                            {
                                throw new DomainExceptionBase(new List<ErrorDto>() { new ErrorDto(403, "Không có quyền truy cập hệ thống") });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DomainExceptionBase(new List<ErrorDto>() { new ErrorDto(403, ex.Message) });
                }
            }

        }
    }
}
