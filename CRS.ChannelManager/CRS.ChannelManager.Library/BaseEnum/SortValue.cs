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
    public enum SortValue
    {
        [Description("ASC")]
        [EnumMember(Value = "ASC")]
        ASC = 0,

        [Description("DESC")]
        [EnumMember(Value = "DESC")]
        DESC = 1,
    }
}
