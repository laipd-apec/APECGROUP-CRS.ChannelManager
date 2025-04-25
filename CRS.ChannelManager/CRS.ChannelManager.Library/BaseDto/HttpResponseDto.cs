using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorCode = CRS.ChannelManager.Library.Constants.ErrorCode;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class HttpResponseDto
    {
        public class OkResponseDto<T> : IHttpResponseDto
        {
            public OkResponseDto(T data, string message)
            {
                Status = HttpResponseStatus.Success;
                Data = data;
                Message = message;
            }
            public HttpResponseStatus Status { get; set; }
            public bool IsSuccess { get; set; }
            public PaginationDto Pagination { get; set; }
            public T Data { get; set; }
            public string? Message { get; set; }
            public OkResponseDto(bool isSuccess, string message, T data, PaginationDto pagination)
            {
                IsSuccess = isSuccess;
                Data = data;
                Pagination = pagination;
            }

        }

        public class ErrorResponseDto<T> : IHttpResponseDto
        {
            public ErrorResponseDto(T data, ErrorCode errorCode, string message)
            {
                Data = data;
                ErrorCode = errorCode;
                Message = message;
                Status = HttpResponseStatus.Error;
            }

            public ErrorResponseDto(DomainExceptionBase ex)
            {
                ErrorCode = ex.ErrorCode;
                Message = ex.Message;
                Status = HttpResponseStatus.Error;
            }

            public ErrorResponseDto(T data, DomainExceptionBase ex)
            {
                ErrorCode = ex.ErrorCode;
                Message = ex.Message;
                Status = HttpResponseStatus.Error;
                Data = data;
            }
            public HttpResponseStatus Status { get; set; }
            public ErrorCode ErrorCode { get; set; }
            public T? Data { get; set; }
            public string? Message { get; set; }

        }
    }

    public interface IHttpResponseDto { }
}
