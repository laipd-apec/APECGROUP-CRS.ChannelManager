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
    [Table("hotel", Schema = "channel_manager")]
    [Description("Lưu danh sách khách sạn, được đồng bộ về từ Inventory")]
    public class HotelEntity : EntityBase
    {
        [Column("hotel_id")]
        [Description("ID khách sạn từ Inventory")]
        [MaxLength(50)]
        [Required]
        public string HotelId { get; set; }

        [Column("code")]
        [Description("Mã khách sạn")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [Column("short_name")]
        [Description("Tên viết tắt")]
        [MaxLength(255)]
        [Required]
        public string ShortName { get; set; }

        [Column("full_name")]
        [Description("Tên đầy đủ")]
        [MaxLength(512)]
        [Required]
        public string FullName { get; set; }

        [Column("sync_key")]
        [Description("mã dùng để đối chiếu dữ liệu với hệ thống master data")]
        public long? SyncKey { get; set; }

        [Column("thumbnail_image")]
        [Description("Ảnh đại diện")]
        public string? ThumbnailImage { get; set; }

    }
}
