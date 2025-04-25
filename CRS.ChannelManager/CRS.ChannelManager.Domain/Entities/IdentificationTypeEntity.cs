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
    [Table("identification_type", Schema = "channel_manager")]
    [Description("Lưu danh sách Danh xưng, được đồng bộ về từ CM")]
    public class IdentificationTypeEntity : EntityBase
    {
        [Column("code")]
        [Description("Mã loại giấy tờ")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }

        [Column("name")]
        [Description("Tên loại giấy tờ")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        [Description("Mô tả")]
        [MaxLength(512)]
        public string? Description { get; set; }

        [Column("document_config")]
        [Description("Định nghĩa danh sách các giấy tờ cần upload")]
        [MaxLength(int.MaxValue)]
        public string? DocumentConfig { get; set; }

        [Column("sync_key")]
        [Description("mã dùng để đối chiếu dữ liệu với hệ thống master data")]
        public long? SyncKey { get; set; }

        [Column("name_unaccent")]
        [Description("Tên bỏ dấu và unicode")]
        [MaxLength(500)]
        public string? NameUnaccent { get; set; }
    }
}
