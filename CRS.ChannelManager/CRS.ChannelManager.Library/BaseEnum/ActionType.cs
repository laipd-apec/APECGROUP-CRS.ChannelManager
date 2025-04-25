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
    public enum ActionType
    {
        [Description("GETONE")]
        [EnumMember(Value = "0")]
        One = 0,

        [Description("GETALL")]
        [EnumMember(Value = "1")]
        All = 1,

        [Description("SEARCH")]
        [EnumMember(Value = "2")]
        Search = 2,

        [Description("CREATE")]
        [EnumMember(Value = "3")]
        Create = 3,

        [Description("UPDATE")]
        [EnumMember(Value = "4")]
        Update = 4,

        [Description("DELETE")]
        [EnumMember(Value = "5")]
        Delete = 5,

        [Description("WAIT")]
        [EnumMember(Value = "6")]
        Wait = 6,

        [Description("APPROVAL")]
        [EnumMember(Value = "7")]
        Approval = 7,

        [Description("REJECT")]
        [EnumMember(Value = "8")]
        Reject = 8,

    }
}
