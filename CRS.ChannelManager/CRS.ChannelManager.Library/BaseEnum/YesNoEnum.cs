using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseEnum
{
    public enum YesNoEnum
    {
        [EnumMember(Value = "N")]
        No = 0,
        [EnumMember(Value = "Y")]
        Yes = 1
    }
}
