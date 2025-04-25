using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CRS.ChannelManager.Library.Base;

namespace CRS.ChannelManager.Domain.Entities
{
    [Table("market_segment", Schema = "channel_manager")]
    [Description("Lưu danh sách Market Segment được đồng bộ về từ CM")]
    public class MarketSegmentEntity : EntityBase
    {
        [Column("code")]
        [Description("Mã khách sạn")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [Column("name")]
        [Description("Tên viết tắt")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        [Description("Tên")]
        [MaxLength(512)]
        public string? Description { get; set; }

        [Column("sync_key")]
        [Description("mã dùng để đối chiếu dữ liệu với hệ thống master data")]
        public long? SyncKey { get; set; }

        [Column("name_unaccent")]
        [Description("Tên bỏ dấu và unicode")]
        [MaxLength(500)]
        public string? NameUnaccent { get; set; }

        //public virtual ICollection<BookingEntity>? Bookings { get; set; }
    }
}
