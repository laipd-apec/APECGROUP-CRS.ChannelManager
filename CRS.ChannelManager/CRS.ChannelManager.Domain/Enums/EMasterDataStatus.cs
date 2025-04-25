using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Enums
{
    public enum EMasterDataStatus
    {
        [Description("Published")]
        [EnumMember(Value = "published")]
        Published = 1,

        [Description("Draft")]
        [EnumMember(Value = "draft")]
        Draft = 2,

        [Description("Archived")]
        [EnumMember(Value = "archived")]
        Archived = 3,
    }
}
