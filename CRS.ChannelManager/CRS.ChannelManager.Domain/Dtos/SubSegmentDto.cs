using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class SubSegmentDto
    {
        public class SubSegmentRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
            public string MarketSegmentCode { get; set; }
        }

        public class SubSegmentCreateDto : SubSegmentRequestDto, ICommand<long>
        {

        }

        public class SubSegmentUpdateDto : SubSegmentRequestDto, ICommand<long>
        {

        }

        public class SubSegmentGetOneDto : IQuery<SubSegmentResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class SubSegmentDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class SubSegmentResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public string MarketSegmentCode { get; set; }
            public long? SyncKey { get; set; }
        }

        public class SubSegmentSearchDto : SearchBaseDto<SubSegmentSearchDto.SubSegmentFilter>, IQuery<PagedResultBaseDto<List<SubSegmentResponseDto>>>
        {
            public class SubSegmentFilter : FilterDtoBase
            {

            }
        }
    }
}
