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
    public class MarketSegmentDto
    {
        public class MarketSegmentRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class MarketSegmentCreateDto : MarketSegmentRequestDto, ICommand<long>
        {

        }

        public class MarketSegmentUpdateDto : MarketSegmentRequestDto, ICommand<long>
        {

        }

        public class MarketSegmentGetOneDto : IQuery<MarketSegmentResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class MarketSegmentDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class MarketSegmentResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class MarketSegmentSearchDto : SearchBaseDto<MarketSegmentSearchDto.MarketSegmentFilter>, IQuery<PagedResultBaseDto<List<MarketSegmentResponseDto>>>
        {
            public class MarketSegmentFilter : FilterDtoBase
            {

            }
        }
    }
}
