using CRS.ChannelManager.Library.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Entities
{
    [Table("room_type", Schema = "channel_manager")]
    [Description("Lưu danh sách khách sạn, được đồng bộ về từ Inventory")]
    public class RoomTypeEntity : EntityBase
    {
        [Column("hotel_id")]
        [Description("ID khách sạn từ Inventory")]
        [MaxLength(50)]
        [Required]
        public string HotelId { get; set; }

        [Column("room_type_id")]
        [Description("ID loại phòng từ Inventory")]
        [MaxLength(50)]
        [Required]
        public string RoomTypeId { get; set; }

        [Column("code")]
        [Description("Mã loại phòng từ Inventory")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [Column("name")]
        [Description("Tên loại phòng từ Inventory")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Column("sync_key")]
        [Description("mã dùng để đối chiếu dữ liệu với hệ thống master data")]
        public long? SyncKey { get; set; }

        [Column("total_adult")]
        [DefaultValue(1)]
        [Description("Tổng người lớn")]
        public int TotalAdult { get; set; }

        [Column("total_child")]
        [Required]
        [DefaultValue(1)]
        [Description("Tổng trẻ em")]
        public int TotalChild { get; set; }

        [Column("thumbnail_image")]
        [Description("Ảnh đại diện")]
        public string? ThumbnailImage { get; set; }

        [Column("room_size")]
        [DefaultValue(1)]
        [Description("Số lượng phòng thuộc hạng phòng")]
        public int? RoomSize  { get; set; }

        [Column("name_unaccent")]
        [Description("Tên bỏ dấu và unicode")]
        [MaxLength(500)]
        public string? NameUnaccent { get; set; }
    }
}
