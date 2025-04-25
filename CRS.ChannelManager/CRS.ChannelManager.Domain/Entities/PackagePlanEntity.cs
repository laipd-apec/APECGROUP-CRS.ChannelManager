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

namespace CRS.ChannelManager.Domain.Entities
{
    [Table("package_plan", Schema = "channel_manager")]
    [Description("Lưu danh sách packgage plan của khách sạn đồng bộ từ masterdata")]
    public class PackagePlanEntity : EntityBase
    {
        [Column("hotel_id")]
        [Description("hotel id lấy từ bảng hotel ở masterdata")]
        [MaxLength(50)]
        public string? HotelId { get; set; }

        [Column("code")]
        [Description("Mã viết tắt")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [Column("name")]
        [Description("Tên")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        [Description("Mô tả thêm")]
        [MaxLength(512)]
        public string? Description { get; set; }

        [Column("sync_key")]
        [Description("mã dùng để đối chiếu dữ liệu với hệ thống master data")]
        public long? SyncKey { get; set; }

        [Column("name_unaccent")]
        [Description("Tên bỏ dấu và unicode")]
        [MaxLength(500)]
        public string? NameUnaccent { get; set; }

        //public virtual ICollection<BookingItemEntity>? BookingItems { get; set; }
    }
}
