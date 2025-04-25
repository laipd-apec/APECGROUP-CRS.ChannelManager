using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.EnumType
{
    public enum AuditType
    {
        [Description("NONE")]
        [EnumMember(Value = "0")]
        None = 0,

        [Description("CREATE")]
        [EnumMember(Value = "1")]
        Create = 1,

        [Description("UPDATE")]
        [EnumMember(Value = "2")]
        Update = 2,

        [Description("DELETE")]
        [EnumMember(Value = "3")]
        Delete = 3
    }
}
