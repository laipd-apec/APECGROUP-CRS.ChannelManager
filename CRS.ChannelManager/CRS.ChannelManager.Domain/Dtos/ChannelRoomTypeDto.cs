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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class ChannelRoomTypeDto
    {
        public class ChannelRoomTypeRequestDto : RequestBaseDto
        {
            public long HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? DisplayRate { get; set; }
            public string? NameUnaccent { get; set; }
        }

        public class ChannelRoomTypeCreateDto : ChannelRoomTypeRequestDto, ICommand<long>
        {

        }

        public class ChannelRoomTypeCreateListDto : ICommand<List<long>>
        {
            public List<ChannelRoomTypeRequestDto> Items { get; set; }
        }

        public class ChannelRoomTypeUpdateDto : ChannelRoomTypeRequestDto, ICommand<long>
        {

        }

        public class ChannelRoomTypeUpdateListDto : ICommand<List<long>>
        {
            public List<ChannelRoomTypeRequestDto> Items { get; set; }
        }

        public class ChannelRoomTypeGetOneDto : IQuery<ChannelRoomTypeResponseDto>
        {
            public long Id { get; set; }
        }

        public class ChannelRoomTypeDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class ChannelRoomTypeDeleteListDto : IQuery<long>
        {
            public List<long> Ids { get; set; }
        }

        public class ChannelRoomTypeResponseDto : ResponseBaseDto
        {
            public ChannelRoomTypeResponseDto()
            {
                Hotel = new HotelDto.HotelResponseDto();
            }

            public long HotelId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public long? DisplayRate { get; set; }
            public string? NameUnaccent { get; set; }
            public HotelDto.HotelResponseDto Hotel { get; set; }

            [JsonIgnore]
            public List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>? ChannelMappingRoomTypes { get; set; }
        }

        public class ChannelRoomTypeSearchDto : SearchBaseDto<ChannelRoomTypeSearchDto.ChannelRoomTypeFilter>, IQuery<PagedResultBaseDto<List<ChannelRoomTypeResponseDto>>>
        {
            public class ChannelRoomTypeFilter : FilterDtoBase
            {

            }
        }
    }
}
