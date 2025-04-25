using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class BaseStatusDto
    {
        public StatusCode Status { get; set; }
        public List<ErrorDto> ListCode { get; set; }

        public BaseStatusDto(StatusCode status, List<ErrorDto> listCode)
        {
            Status = status;
            ListCode = listCode;
        }
        public BaseStatusDto()
        {

        }
    }

    public enum StatusCode
    {
        Fail = 0,
        Success = 1,
        Waring = 2,
    }

}
