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
    [Table("account", Schema = "channel_manager")]
    [Description("Lưu danh sách các Travel Agent, được đồng bộ từ CM")]
    public class AccountEntity : EntityBase
    {
        [Column("hotel_id")]
        [Description("Id hotel lưu trên hệ thống channel manager")]
        public long HotelId { get; set; }

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

        [Column("tax_code")]
        [Description("Mã số thuế")]
        [MaxLength(500)]
        public string? TaxCode { get; set; }

        [Column("tax_name")]
        [Description("Tên mã số thuế")]
        [MaxLength(500)]
        public string? TaxName { get; set; }

        [Column("address")]
        [Description("Địa chỉ")]
        [MaxLength(500)]
        public string? Address { get; set; }

        [Column("phone")]
        [Description("Điện thoại liên hệ")]
        [MaxLength(500)]
        public string? Phone { get; set; }

        [Column("email")]
        [Description("email liên hệ")]
        [MaxLength(500)]
        public string? Email { get; set; }

        [Column("market_segment_id")]
        [Description("Id market segment lưu trên hệ thống masterdata")]
        public long MarketSegmentId { get; set; }

        [Column("description")]
        [Description("Ghi chú thêm")]
        [MaxLength(512)]
        public string? Description { get; set; }

        [Column("name_unaccent")]
        [Description("Tên bỏ dấu và unicode")]
        [MaxLength(500)]
        public string? NameUnaccent { get; set; }

        [Column("sync_key")]
        [Description("mã dùng để đối chiếu dữ liệu với hệ thống master data")]
        public long? SyncKey { get; set; }

    }
}
