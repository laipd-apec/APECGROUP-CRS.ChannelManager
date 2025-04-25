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
    public class ServiceDto
    {
        public class ServiceRequestDto : RequestBaseDto
        {
            public string? HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class ServiceCreateDto : ServiceRequestDto, ICommand<long>
        {

        }

        public class ServiceUpdateDto : ServiceRequestDto, ICommand<long>
        {

        }

        public class ServiceGetOneDto : IQuery<ServiceResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class ServiceDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class ServiceResponseDto : ResponseBaseDto
        {
            public string? HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class ServiceSearchDto : SearchBaseDto<ServiceSearchDto.ServiceFilter>, IQuery<PagedResultBaseDto<List<ServiceResponseDto>>>
        {
            public class ServiceFilter : FilterDtoBase
            {

            }
        }
    }
}
