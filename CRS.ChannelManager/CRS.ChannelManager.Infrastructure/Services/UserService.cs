using CRS.ChannelManager.Domain.Constants;
using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.Utils;
using Elastic.Clients.Elasticsearch.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserInfoDto.UserInfoResponseDto?> GetUserInfoAsync();

        Task<UserInfoDto.LoginResponseDto> LoginSSO(UserInfoDto.LoginRequestDto model);

        Task<List<UserInfoDto.UserHotelResponseDto>> UserHotel(string cmHotelId);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IOptions<ConfigSettingDto.PermissionConfig> _permissionConfig;
        private readonly IOptions<ConfigSettingDto.SSOConfig> _ssoConfig;
        private IHttpContextAccessor _httpContext;
        public UserService(
            IHttpContextAccessor httpContext
            , IOptions<ConfigSettingDto.PermissionConfig> permissionConfig
            , IOptions<ConfigSettingDto.SSOConfig> ssoConfig
            , ILogger<UserService> logger)
        {
            _httpContext = httpContext;
            _permissionConfig = permissionConfig;
            _ssoConfig = ssoConfig;
            _logger = logger;
        }

        public async Task<UserInfoDto.UserInfoResponseDto?> GetUserInfoAsync()
        {
            UserInfoDto.UserInfoResponseDto? user = null;
            try
            {
                var hearder = new Dictionary<string, string>();
                string token = CommonUtils.GetCurrentToken(_httpContext);
                if (!string.IsNullOrEmpty(token))
                {
                    hearder.Add(HeaderNames.Authorization, $"Bearer {token}");
                    string domain = _permissionConfig.Value.IpServer;
                    string router = _permissionConfig.Value.ApiUserInfo;
                    var result = await HttpUtils.GetJson<ResultBaseDto<UserInfoDto.UserInfoResponseDto>>(domain, router, hearder);
                    if (result != null)
                    {
                        user = result.Data;
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                return user;
                //throw new DomainExceptionBase(ex.Message);
            }
        }

        public async Task<UserInfoDto.LoginResponseDto> LoginSSO(UserInfoDto.LoginRequestDto model)
        {
            try
            {
                UserInfoDto.LoginResponseDto login = new UserInfoDto.LoginResponseDto();
                var hearder = new Dictionary<string, string>();
                var addParameter = new Dictionary<string, string>();
                addParameter.Add("client_id", _ssoConfig.Value.GrantTypeConfig.ClientId);
                addParameter.Add("client_secret", _ssoConfig.Value.GrantTypeConfig.ClientSecret);
                addParameter.Add("grant_type", _ssoConfig.Value.GrantTypeConfig.GrantType);
                addParameter.Add("username", model.UserName);
                addParameter.Add("password", model.Password);
                string domain = _ssoConfig.Value.Domain;
                string router = _ssoConfig.Value.UrlApiConnect;
                var result = await HttpUtils.PostForm<UserInfoDto.LoginResponseDto>(domain, router, hearder, addParameter, _logger);
                if (result == null)
                {
                    throw new DomainExceptionBase(new List<ErrorDto>() { new ErrorDto(CrsErrorCode.Anauthorized, CrsErrorCode.AnauthorizedMsg) });
                }
                login = result;
                return login;
            }
            catch (DomainExceptionBase ex)
            {
                throw new DomainExceptionBase(ex.ErrorDtos);
            }
        }

        public async Task<List<UserInfoDto.UserHotelResponseDto>> UserHotel(string cmHotelId)
        {
            try
            {
                List<UserInfoDto.UserHotelResponseDto> data = new List<UserInfoDto.UserHotelResponseDto>();
                var hearder = new Dictionary<string, string>();
                string domain = _permissionConfig.Value.IpServer;
                string router = _permissionConfig.Value.ApiUserHotel;
                var hotelSearch = new HotelDto.HotelSearchDto()
                {
                    Pagination = null,
                    Filter = new HotelDto.HotelSearchDto.HotelFilter()
                    {
                        FilterGroup = new List<FilterBase.FilterGroup>()
                    {
                        new FilterBase.FilterGroup()
                        {
                            Condition = "And",
                            Filters = new List<FilterBase.Filter>()
                            {
                                new FilterBase.Filter()
                                {
                                    PropertyName = "cmHotelId",
                                    PropertyType = "string",
                                    Operator = "==",
                                    PropertyValue = cmHotelId
                                }
                            }
                        }
                    }
                    },
                };
                var result = await HttpUtils.PostJson<HotelDto.HotelSearchDto, ResultBaseDto<List<UserInfoDto.UserHotelResponseDto>>>(domain, router, hearder, hotelSearch, _logger);
                if (result != null)
                {
                    data = result.Data;
                }
                return data;
            }
            catch (DomainExceptionBase ex)
            {
                return new List<UserInfoDto.UserHotelResponseDto>();
            }
        }
    }

}
