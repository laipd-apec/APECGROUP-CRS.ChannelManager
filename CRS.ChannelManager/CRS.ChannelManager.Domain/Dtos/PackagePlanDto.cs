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
    public class PackagePlanDto
    {
        public class PackagePlanRequestDto : RequestBaseDto
        {
            public string? HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class PackagePlanCreateDto : PackagePlanRequestDto, ICommand<long>
        {

        }

        public class PackagePlanUpdateDto : PackagePlanRequestDto, ICommand<long>
        {

        }

        public class PackagePlanGetOneDto : IQuery<PackagePlanResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class PackagePlanDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class PackagePlanResponseDto : ResponseBaseDto
        {
            public string? HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class PackagePlanSearchDto : SearchBaseDto<PackagePlanSearchDto.PackagePlanFilter>, IQuery<PagedResultBaseDto<List<PackagePlanResponseDto>>>
        {
            public class PackagePlanFilter : FilterDtoBase
            {

            }
        }
    }
}
