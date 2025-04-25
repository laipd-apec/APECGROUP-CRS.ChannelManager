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
    public enum SortMethod
    {
        [Description("OrderBy")]
        [EnumMember(Value = "OrderBy")]
        OrderBy = 0,

        [Description("OrderByDescending")]
        [EnumMember(Value = "OrderByDescending")]
        OrderByDescending = 1,

        [Description("ThenBy")]
        [EnumMember(Value = "ThenBy")]
        ThenBy = 2,

        [Description("ThenByDescending")]
        [EnumMember(Value = "ThenByDescending")]
        ThenByDescending = 3,
    }
}
