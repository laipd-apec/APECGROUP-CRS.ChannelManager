using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class HotelDto
    {
        public class HotelRequestDto : RequestBaseDto
        {
            public string HotelId { get; set; }
            public string Code { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
            public long? SyncKey { get; set; }
            public string? ThumbnailImage { get; set; }
        }

        public class HotelCreateDto : HotelRequestDto, ICommand<long>
        {

        }

        public class HotelUpdateDto : HotelRequestDto, ICommand<long>
        {

        }

        public class HotelGetOneDto : IQuery<HotelResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class HotelDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class HotelResponseDto : ResponseBaseDto
        {
            public string HotelId { get; set; } 
            public string Code { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
            public long? SyncKey { get; set; }
            public string? ThumbnailImage { get; set; }
        }

        public class HotelSearchDto : SearchBaseDto<HotelSearchDto.HotelFilter>, IQuery<PagedResultBaseDto<List<HotelResponseDto>>>
        {
            public class HotelFilter : FilterDtoBase
            {

            }
        }
    }
}
