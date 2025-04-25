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
    public class TravelAgentDto
    {
        public class TravelAgentRequestDto : RequestBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class TravelAgentCreateDto : TravelAgentRequestDto, ICommand<long>
        {

        }

        public class TravelAgentUpdateDto : TravelAgentRequestDto, ICommand<long>
        {

        }

        public class TravelAgentGetOneDto : IQuery<TravelAgentResponseDto>
        {
            //[FromQuery]
            public long Id { get; set; }
        }

        public class TravelAgentDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class TravelAgentResponseDto : ResponseBaseDto
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? SyncKey { get; set; }
        }

        public class TravelAgentSearchDto : SearchBaseDto<TravelAgentSearchDto.TravelAgentFilter>, IQuery<PagedResultBaseDto<List<TravelAgentResponseDto>>>
        {
            public class TravelAgentFilter : FilterDtoBase
            {

            }
        }
    }
}
