using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class FilterBaseDto<T>
    {
        public string QuerySearch { get;set; }
        public T Filter { get; set; }
        public PagingDto Paging { get; set; }
    }

    public class PagingDto
    {
        public int PageSize { get; set; } = 25;
        public int PageIndex { get; set; } = 1;
    }
}
