using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class UserInfoDto
    {
        public class UserInfoResponseDto : ResponseBaseDto
        {
            public string? Avatar { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string FirstName { get; set; }

            public string? LastName { get; set; } = string.Empty;

            public string? Gender { get; set; }

            public DateTime? BirthDay { get; set; }

            public string? Phone { get; set; }

            public string? Address { get; set; }

            public string? Country { get; set; }

            public string? City { get; set; }

            public string? OTP { get; set; }

            public DateTime? OTPExpried { get; set; }

            public string? District { get; set; }

            public string? Subdistrict { get; set; }

            public bool? IsAdmin { get; set; }

            public DateTime? StartConfirmOTP { get; set; }

            public int? TotalConfirm { get; set; }

            public string? ReferCode { get; set; }

            public string? FullName { get; set; }

            public int? AccountId { get; set; }

            public string? AccountCode { get; set; }

        }

        public class LoginRequestDto : ICommand<LoginResponseDto>
        {
            public string UserName { get; set; }

            public string Password { get; set; }
        }

        public class LoginResponseDto
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_expires_in")]
            public int RefreshExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("not-before-policy")]
            public int NotBeforePolicy { get; set; }

            [JsonProperty("session_state")]
            public string SessionState { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }
        }

        public class UserHotelResponseDto
        {
            public int HotelId { get; set; }
            public string HotelCode { get; set; }
            public string? CMHotelId { get; set; }
            public int UserId { get; set; }
            public UserInfoResponseDto User { get; set; }
        }

    }
}
