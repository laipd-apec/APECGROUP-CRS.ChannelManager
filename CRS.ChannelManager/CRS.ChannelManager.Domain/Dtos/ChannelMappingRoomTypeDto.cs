using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class ChannelMappingRoomTypeDto
    {
        public class ChannelMappingRoomTypeRequestDto : RequestBaseDto
        {
            public long ChannelRoomTypeId { get; set; }
            public long HotelId { get; set; }
            public long ChannelId { get; set; }
            public long AccountId { get; set; }
            public long ProductId { get; set; }
            public long PackagePlanId { get; set; }
            public long RoomTypeId { get; set; }
            public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
        }

        public class ChannelMappingRoomTypeCreateDto : ChannelMappingRoomTypeRequestDto, ICommand<long>
        {

        }

        public class ChannelMappingRoomTypeCreateListDto : ICommand<List<long>>
        {
            public List<ChannelMappingRoomTypeRequestDto> Items { get; set; }
        }

        public class ChannelMappingRoomTypeUpdateDto : ChannelMappingRoomTypeRequestDto, ICommand<long>
        {

        }

        public class ChannelMappingRoomTypeUpdateListDto : ICommand<List<long>>
        {
            public List<ChannelMappingRoomTypeRequestDto> Items { get; set; }
        }

        public class ChannelMappingRoomTypeGetOneDto : IQuery<ChannelMappingRoomTypeResponseDto>
        {
            public long Id { get; set; }
        }

        public class ChannelMappingRoomTypeDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class ChannelMappingRoomTypeDeleteListDto : IQuery<long>
        {
            public long Ids { get; set; }
        }

        public class ChannelMappingRoomTypeResponseDto : ResponseBaseDto
        {
            public ChannelMappingRoomTypeResponseDto()
            {
                Channel = new ChannelDto.ChannelResponseDto();
                Hotel = new HotelDto.HotelResponseDto();
                ChannelRoomType = new ChannelRoomTypeDto.ChannelRoomTypeResponseDto();
                Product = new ProductDto.ProductResponseDto();
                PackagePlan = new PackagePlanDto.PackagePlanResponseDto();
                RoomType = new RoomTypeDto.RoomTypeResponseDto();
            }

            public long ChannelRoomTypeId { get; set; }
            public long HotelId { get; set; }
            public long ChannelId { get; set; }
            public long AccountId { get; set; }
            public long ProductId { get; set; }
            public long PackagePlanId { get; set; }
            public long RoomTypeId { get; set; }
            public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();
            public ChannelRoomTypeDto.ChannelRoomTypeResponseDto ChannelRoomType { get; set; }
            public ChannelDto.ChannelResponseDto Channel { get; set; }
            public HotelDto.HotelResponseDto Hotel { get; set; }
            public ProductDto.ProductResponseDto Product { get; set; }
            public PackagePlanDto.PackagePlanResponseDto PackagePlan { get; set; }
            public RoomTypeDto.RoomTypeResponseDto RoomType { get; set; }
        }

        public class ChannelMappingRoomTypeSearchDto : SearchBaseDto<ChannelMappingRoomTypeSearchDto.ChannelMappingRoomTypeFilter>, IQuery<PagedResultBaseDto<List<ChannelMappingRoomTypeResponseDto>>>
        {
            public class ChannelMappingRoomTypeFilter : FilterDtoBase
            {

            }
        }
    }
}
