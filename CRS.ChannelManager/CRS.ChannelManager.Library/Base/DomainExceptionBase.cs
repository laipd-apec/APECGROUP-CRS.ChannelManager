using CRS.ChannelManager.Library.Constants;
using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CRS.ChannelManager.Library.Base
{
    public class DomainExceptionBase : Exception
    {
        public ErrorCode ErrorCode { get; set; }

        private readonly ILogger? _logger = null;

        public DomainExceptionBase(ErrorCode errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
            if (ErrorDtos == null)
            {
                ErrorDtos = new List<ErrorDto>();
            }
            ErrorDtos.Add(new ErrorDto(message));
            WriteLog();
        }

        public List<ErrorDto> ErrorDtos { get; set; }

        public DomainExceptionBase(List<ErrorDto> errorDtos, ILogger? logger = null) : base()
        {
            ErrorDtos = errorDtos;
            _logger = logger;
            WriteLog();
        }

        public DomainExceptionBase(string message, List<ErrorDto> errorDtos, ILogger? logger = null) : base(message)
        {
            ErrorDtos = errorDtos;
            _logger = logger;
            WriteLog();
        }

        //public AppException(string message, Exception inner) : base(message, inner) { }

        public DomainExceptionBase(string message, List<ErrorDto> errorDtos, ILogger? logger = null, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
            ErrorDtos = errorDtos;
            _logger = logger;
            WriteLog();
        }

        public DomainExceptionBase(string? message, ILogger? logger = null) : base(message)
        {
            if (ErrorDtos == null || ErrorDtos.Count == 0)
            {
                ErrorDtos = new List<ErrorDto>();
            }
            ErrorDtos.Add(new ErrorDto(500, message));
            _logger = logger;
            WriteLog();
        }

        private void WriteLog()
        {
            if (_logger != null && this.ErrorDtos.Any())
            {
                _logger.LogError($"LogError{JsonConvert.SerializeObject(this.ErrorDtos)}");
            }
        }
    }
}
