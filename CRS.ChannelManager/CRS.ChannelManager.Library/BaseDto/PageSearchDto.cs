using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class SearchBaseDto<F> where F : FilterDtoBase
    {
        public PageSearchDto? Pagination { get; set; }

        public F? Filter { get; set; }
    }

    public class PageSearchDto
    {
        [Required(ErrorMessage = "Page Index is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1")]
        public int PageIndex { get; set; }

        [Required(ErrorMessage = "Page Size is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1")]
        public int PageSize { get; set; }

        public bool IsAll { get; set; } = false;
    }
}
