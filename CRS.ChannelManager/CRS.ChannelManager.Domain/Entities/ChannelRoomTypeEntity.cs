using CRS.ChannelManager.Library.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CRS.ChannelManager.Domain.Entities
{
    [Table("channel_room_type", Schema = "channel_manager")]
    [Description("Lưu cấu hình mapping channel với dữ liệu master data")]
    [Index(nameof(Code), IsUnique = true)]
    public class ChannelRoomTypeEntity : EntityBase
    {
        public ChannelRoomTypeEntity()
        {
            //Hotel = new HotelEntity();
        }


        [Column("hotel_id")]
        [Description("Id hotel lưu trên hệ thống channel manager")]
        public long HotelId { get; set; }

        [Column("code")]
        [Description("Mã viết tắt")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [Column("name")]
        [Description("Tên channel room type")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        [Description("Mô tả thêm")]
        [MaxLength(int.MaxValue)]
        public string? Description { get; set; }

        [Column("display_rate")]
        [Description("Giá tượng trưng dùng để hiện thị")]
        public long? DisplayRate { get; set; }

        [Column("name_unaccent")]
        [Description("Tên bỏ dấu và unicode")]
        [MaxLength(500)]
        public string? NameUnaccent { get; set; }

        public virtual HotelEntity Hotel { get; set; }

        [JsonIgnore]
        public virtual ICollection<ChannelMappingRoomTypeEntity>? ChannelMappingRoomTypes { get; set; }
    }
}
