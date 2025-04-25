using CRS.ChannelManager.Library.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class AdvanceFilterDto
    {
        public string Name { get; set; }
        public FilterOperator Operator { get; set; }
        public dynamic Value { get; set; }
    }
}
