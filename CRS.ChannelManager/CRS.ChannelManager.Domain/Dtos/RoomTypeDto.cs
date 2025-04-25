using CRS.ChannelManager.Domain.Dtos.Interfaces;
using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class RoomTypeDto
    {
        public class RoomTypeRequestDto : RequestBaseDto
        {
            [Description("ID khách sạn từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string HotelId { get; set; }

            [Description("ID loại phòng từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string RoomTypeId { get; set; }

            [Description("Mã loại phòng từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string Code { get; set; }

            [Description("Tên loại phòng từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string Name { get; set; }

            public long? SyncKey { get; set; }

            public int TotalAdult { get; set; } = 1;
            public int TotalChild { get; set; } = 1;
            public int RoomSize { get; set; } = 1;
            public string? ThumbnailImage { get; set; }
        }

        public class RoomTypeCreateDto : RoomTypeRequestDto, ICommand<long>
        {

        }

        public class RoomTypeUpdateDto : RoomTypeRequestDto, ICommand<long>
        {

        }

        public class RoomTypeGetOneDto : IQuery<RoomTypeResponseDto>
        {
            public long Id { get; set; }
        }

        public class RoomTypeDeleteDto : IQuery<long>
        {
            public long Id { get; set; }
        }

        public class RoomTypeResponseDto : ResponseBaseDto
        {
            [Description("ID khách sạn từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string HotelId { get; set; }

            [Description("ID loại phòng từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string RoomTypeId { get; set; }

            [Description("Mã loại phòng từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string Code { get; set; }

            [Description("Tên loại phòng từ Inventory")]
            [MaxLength(50)]
            [Required]
            public string Name { get; set; }

            public long? SyncKey { get; set; }
            public int TotalAdult { get; set; } = 1;
            public int TotalChild { get; set; } = 1;
            public int RoomSize { get; set; } = 1;
            public string? ThumbnailImage { get; set; }
        }

        public class RoomTypeSearchDto : SearchBaseDto<RoomTypeSearchDto.RoomTypeFilter>, IQuery<PagedResultBaseDto<List<RoomTypeResponseDto>>>
        {
            public class RoomTypeFilter : FilterDtoBase
            {

            }
        }
    }
}
