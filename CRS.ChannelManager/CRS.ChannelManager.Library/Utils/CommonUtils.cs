using CRS.ChannelManager.Library.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Utils
{
    public static class CommonUtils
    {
        public static string GetCurrentUsername(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var usr = httpContextAccessor.HttpContext.User?.Identity?.Name;
                if (string.IsNullOrEmpty(usr))
                {
                    var claimsUser = httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                    if (claimsUser != null)
                        usr = claimsUser.Value.ToString();
                }
                return usr ?? string.Empty;
            }
            catch (Exception ex)
            {
                return "root";
                //throw new DomainExceptionBase(ex.Message);
            }

        }

        public static int GetCurrentEmployee(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                int userId = 0;
                string strUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value ?? string.Empty;
                int.TryParse(strUserId, out userId);
                return userId;
            }
            catch (Exception ex)
            {
                return 0;
                //throw new DomainExceptionBase(ex.Message);
            }

        }

        public static string GetCurrentToken(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var bearer_token = httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                return bearer_token;
            }
            catch (Exception ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }
        }

        public static string GetCurrentRefreshToken(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var bearer_token = httpContextAccessor.HttpContext?.Request.Headers["refresh_token"].ToString().Trim();
                return bearer_token;
            }
            catch (Exception ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }

        }
    }
}
