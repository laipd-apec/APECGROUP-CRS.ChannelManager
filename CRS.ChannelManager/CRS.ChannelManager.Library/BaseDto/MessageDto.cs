using CRS.ChannelManager.Library.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class MessageDto : EntityBase
    {
        public class MessageRequestDto : RequestBaseDto
        {
            public string CodeLanguage { get; set; }

            public string Code { get; set; }

            public string Message { get; set; }
        }

        public class MessageResponseDto : ResponseBaseDto
        {
            public string CodeLanguage { get; set; }

            public string Code { get; set; }

            public string Message { get; set; }
        }


        public class MessageSearchDto : SearchBaseDto<MessageSearchDto.MessageFilter>
        {
            public class MessageFilter : FilterDtoBase
            {

            }
        }
    }
}
