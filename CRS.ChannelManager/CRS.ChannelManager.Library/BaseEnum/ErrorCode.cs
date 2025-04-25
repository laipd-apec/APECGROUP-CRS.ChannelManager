using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseEnum
{
    public enum ErrorCode
    {
        [Display(Name = "Duplicate Data")]
        DUPLICATE_RECORD = 10001,
        [Display(Name = "Duplicate Data")]
        INVALID = 10002,
    }
}
