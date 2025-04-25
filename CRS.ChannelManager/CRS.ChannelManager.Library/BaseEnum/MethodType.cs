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
    public enum MethodType
    {
        [Description("GET")]
        [EnumMember(Value = "GET")]
        GET = 0,

        [Description("POST")]
        [EnumMember(Value = "1")]
        POST = 1,

        [Description("PUT")]
        [EnumMember(Value = "2")]
        PUT = 2,

        [Description("DELETE")]
        [EnumMember(Value = "3")]
        DELETE = 3,

    }
}
