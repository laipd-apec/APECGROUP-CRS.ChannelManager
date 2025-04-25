using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRS.ChannelManager.Library.Base
{
    public abstract class EntityBase
    {
        //public EntityBase()
        //{
        //    CreatedBy = "root";

        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Description("Mã tự tăng")]
        [Column("id")]
        public virtual long Id { get; set; }

        [Column("created_by", TypeName = "varchar")]
        [Description("Người tạo")]
        [MaxLength(255)]
        [IgnoreValidate]
        public string CreatedBy { get; set; }

        [Column("created_date", TypeName = "timestamp")]
        [Description("Ngày tạo")]
        [Required]
        [DefaultValue(typeof(DateTime))]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date", TypeName = "timestamp")]
        [Description("Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }

        [MaxLength(255)]
        [Column("modified_by")]
        [Description("Người sửa")]
        public string? ModifiedBy { get; set; }

        [MaxLength(1), DefaultValue("N")]
        [Description("Trạng thái xóa")]
        [Column("deleted")]
        public string? Deleted { get; set; }

        [Column("deleted_date", TypeName = "timestamp")]
        [Description("Ngày xóa")]
        public DateTime? DeletedDate { get; set; }

        [MaxLength(255)]
        [Column("deleted_by")]
        [Description("Người xóa")]
        public string? DeletedBy { get; set; }
    }

    public abstract class EntityBaseEvent : EntityBase
    {
        private List<DomainEventBase> _events;
        public IReadOnlyList<DomainEventBase> Events => _events.AsReadOnly();

        protected void AddEvent(DomainEventBase @event)
        {
            _events.Add(@event);
        }

        protected void RemoveEvent(DomainEventBase @event)
        {
            _events.Remove(@event);
        }
    }

    public interface IStatusSupportEntity
    {

        string Status { get; set; }
    }

    public interface ICodeSupportEntity
    {
        string Code { get; set; }
    }

    public class UniqueValidate : Attribute
    {
    }

    public class ImportCode : Attribute
    {
    }

    public class IgnoreValidate : Attribute
    {
    }

    public class UniqueFormatValidate : Attribute
    {
        [Description("Định nghĩa mẫu tự sinh theo cấu trúc các lớp các với nhau bằng dấu :, ví dụ UNIQ+{YYMMDD}+{SEQ}")]
        public string Format { get; set; }
        public UniqueFormatValidate(string format)
        {
            Format = format;
        }
    }
}