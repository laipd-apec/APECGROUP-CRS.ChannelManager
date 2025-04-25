using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class AdvanceSearchFilterDto
    {
        public string? Condition { get; set; }
        public List<AdvanceSearchRule>? Rules { get; set; }
    }

    public class AdvanceSearchRule
    {
        public string? Label { get; set; }
        public string? Field { get; set; }
        public string? Operator { get; set; }
        public string? Type { get; set; }
        public dynamic? Value { get; set; }

        public string? Condition { get; set; }
        public List<AdvanceSearchRule>? Rules { get; set; }
    }
}
