using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class AuditDto
    {
        public class AuditRequestDto : RequestBaseDto
        {
            public string UserName { get; set; } = string.Empty;

            public string Type { get; set; } = string.Empty;

            public string TableName { get; set; } = string.Empty;

            public string? OldValues { get; set; }

            public string? NewValues { get; set; }

            public string? AffectedColumns { get; set; }

            public string? PrimaryKey { get; set; }

            public string? Data { get; set; }
        }

        public class AuditResponseDto : ResponseBaseDto
        {
            public string UserName { get; set; } = string.Empty;

            public string Type { get; set; } = string.Empty;

            public string TableName { get; set; } = string.Empty;

            public string? OldValues { get; set; }

            public string? NewValues { get; set; }

            public string? AffectedColumns { get; set; }

            public string? PrimaryKey { get; set; }

            public string? Data { get; set; }
        }

        public class AuditSearchDto : SearchBaseDto<AuditSearchDto.AuditFilter>
        {
            public class AuditFilter : FilterDtoBase
            {

            }
        }
    }
}
