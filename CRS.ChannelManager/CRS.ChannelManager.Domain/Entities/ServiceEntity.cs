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
    [Table("service", Schema = "channel_manager")]
    [Description("Lưu danh sách service của khách sạn")]
    public class ServiceEntity : EntityBase
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

        //[Column("status")]
        //[Description("Trạng thái hoạt động")]
        //[MaxLength(1)]
        //public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();

        //public virtual ICollection<BookingItemEntity>? BookingItems { get; set; }
    }
}
