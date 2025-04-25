using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.ChannelManager.Library.Base;

namespace CRS.ChannelManager.Library.BaseEntities
{

    //[Table("AUDIT_LOG")]
    public class AuditEntity : EntityBase
    {
        [Column("USERNAME"), MaxLength(50), Description("Người thao tác"), Required]
        public string UserName { get; set; } = String.Empty;


        [Column("TYPE"), MaxLength(50), Description("Hành động"), Required]
        public string Type { get; set; } = String.Empty;


        [Column("TABLE_NAME"), MaxLength(100), Description("Thao tác trên bảng nào"), Required]
        public string TableName { get; set; } = String.Empty;


        [Column("OLD_VALUES"), MaxLength(int.MaxValue), Description("Dữ liệu trước")]
        public string? OldValues { get; set; }


        [Column("NEW_VALUES"), MaxLength(int.MaxValue), Description("Dữ liệu sau")]
        public string? NewValues { get; set; }


        [Column("AFFECTED_COLUMNS"), MaxLength(int.MaxValue), Description("Cột thay đổi")]
        public string? AffectedColumns { get; set; }


        [Column("PRIMARY_KEY"), MaxLength(500), Description("Mã của trường thay đổi")]
        public string? PrimaryKey { get; set; }


        [Column("DATA"), MaxLength(int.MaxValue), Description("Dữ liệu gửi lên")]
        public string? Data { get; set; }
    }
}
