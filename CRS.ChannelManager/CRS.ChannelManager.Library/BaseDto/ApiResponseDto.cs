using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class ApiResponseDto<T>
    {
        public ApiResponseDto() { }

        public ApiResponseDto(T data)
        {
            Data = data;
        }

        public ApiResponseDto(T data, string? messageCode, string? messageName)
        {
            Data = data;
            MessageCode = messageCode;
            MessageName = messageName;
        }

        public string? MessageCode { get; set; }
        public string? MessageName { get; set; }
        public T Data { get; set; }
    }
}
