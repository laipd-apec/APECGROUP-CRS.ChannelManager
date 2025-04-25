using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    /// <summary>
    /// Represents a result dto
    /// </summary>
    public partial class PagedResultBaseDto<T>
    {
        public T? Result { get; set; }

        public PaginationDto? Pagination { get; set; }
    }

    public partial class PaginationDto
    {

        public int PageCurrent { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public PaginationDto()
        {

        }

        public PaginationDto(int currentPage, int pageSize, int rowCount)
        {

            PageCurrent = currentPage;
            PageSize = pageSize;
            RowCount = rowCount;
            var pageCount = (double)rowCount / pageSize;
            PageCount = (int)Math.Ceiling(pageCount);
        }

        public int FirstRowOnPage
        {
            get { return RowCount == 0 ? RowCount : (PageCurrent - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(PageCurrent * PageSize, RowCount); }
        }
    }
}
