using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class RequestResponseLogDto
    {
        public class RequestResponseLogRequestDto : RequestBaseDto
        {
            public string Url { get; set; }
            public string Method { get; set; }
            public string Header { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
        }

        public class RequestResponseLogResponseDto : ResponseBaseDto
        {
            public string Url { get; set; }
            public string Method { get; set; }
            public string Header { get; set; }
            public string Request { get; set; }
            public string Response { get; set; }
        }

        public class RequestResponseLogSearchDto : SearchBaseDto<RequestResponseLogSearchDto.RequestResponseLogFilter>
        {
            public class RequestResponseLogFilter : FilterDtoBase
            {

            }
        }

    }
}
