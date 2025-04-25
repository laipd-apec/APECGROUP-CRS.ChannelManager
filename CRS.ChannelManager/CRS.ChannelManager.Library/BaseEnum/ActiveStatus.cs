using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseEnum
{
    public enum ActiveStatus
    {
        [Description("Active")]
        [EnumMember(Value = "A")]
        Active = 0,

        [Description("InActive")]
        [EnumMember(Value = "I")]
        InActive = 1,

        [Description("Close")]
        [EnumMember(Value = "C")]
        Close = 2
    }
}