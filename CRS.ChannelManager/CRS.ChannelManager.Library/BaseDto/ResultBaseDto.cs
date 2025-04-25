using CRS.ChannelManager.Library.BaseDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public partial class ResultBaseDto<T> : ResultDto
    {

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("pagination")]
        public PaginationDto Pagination { get; set; }

        public ResultBaseDto()
        {
            Errors = null;
        }

        public ResultBaseDto(bool isSuccess)
            : this(isSuccess, string.Empty, default)
        {
        }


        public ResultBaseDto(bool isSuccess, string message)
            : this(isSuccess, message, default)
        {
        }

        public ResultBaseDto(bool isSuccess, List<ErrorDto> message)
           : base(isSuccess, message)
        {
        }

        public ResultBaseDto(bool isSuccess, List<ErrorDto> message, T data)
           : base(isSuccess, message)
        {
            Data = data;
        }

        public ResultBaseDto(bool isSuccess, string message, T data)
            : base(isSuccess, message)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        public ResultBaseDto(bool isSuccess, string message, ICollection<ErrorDto> errors, T data)
            : base(isSuccess, message)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Data = data;
        }

        public ResultBaseDto(bool isSuccess, string message, T data, PaginationDto pagination)
            : base(isSuccess, message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Pagination = pagination;
        }

    }

    public partial class ResultDto
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("errors")]
        public ICollection<ErrorDto> Errors { get; set; }

        public ResultDto()
        {

        }

        public ResultDto(bool isSuccess)
            : this(isSuccess, string.Empty)
        {
        }

        public ResultDto(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            AddErrorMessage(message);
        }

        public ResultDto(bool isSuccess, List<ErrorDto> message)
        {
            IsSuccess = isSuccess;
            AddListErrorMessage(message);
        }

        public ResultDto(bool isSuccess, List<ErrorDto> message, dynamic data)
        {
            IsSuccess = isSuccess;
            AddListErrorMessage(message);

        }

        public ResultDto(bool isSuccess, ICollection<ErrorDto> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public void AddErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (Errors == null)
                    Errors = new List<ErrorDto>();

                Errors.Add(new ErrorDto(message));
            }
        }

        public void AddListErrorMessage(List<ErrorDto> message)
        {
            if (!string.IsNullOrEmpty(message[0].Message))
            {
                if (Errors == null)
                    Errors = new List<ErrorDto>();

                Errors = message;
            }
        }
    }

    public class ErrorDto
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        //public ErrorDto()
        //{
        //}
        public ErrorDto(string message)
        {
            Message = message;
        }

        public ErrorDto(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public ErrorDto(int id, int code, string message)
        {
            Id = id;
            Code = code;
            Message = message;
        }
    }

}
