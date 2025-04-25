using CRS.ChannelManager.Library.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class FilterDtoBase
    {
        public string? Sort { get; set; }

        //public string? Filter { get; set; }

        //public string? QuerySearch { get; set; }

        public string? TextSearch { get; set; }

        //public string? AdvanceSearch { get; set; }

        public List<FilterBase.FilterGroup>? FilterGroup { get; set; }
        public FilterBase.MoreFilterGroup? MoreFilterGroup { get; set; }
    }
}
