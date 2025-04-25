using CRS.ChannelManager.Library.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CRS.ChannelManager.Library.BaseEnum;
using CRS.ChannelManager.Library.Utils;

namespace CRS.ChannelManager.Domain.Entities
{
    [Table("channel_mapping_room_type", Schema = "channel_manager")]
    [Description("Lưu cấu hình mapping channel room type với dữ liệu master data")]
    public class ChannelMappingRoomTypeEntity : EntityBase
    {
        public ChannelMappingRoomTypeEntity()
        {
            //Hotel = new HotelEntity();
            //Channel = new ChannelEntity();
        }

        [Column("channel_room_type_id")]
        [Description("id channel room type lưu trên hệ thống channel manager")]
        public long ChannelRoomTypeId { get; set; }

        [Column("hotel_id")]
        [Description("Id hotel lưu trên hệ thống channel manager")]
        public long HotelId { get; set; }

        [Column("channel_id")]
        [Description("Id channel lưu trên hệ thống channel manager")]
        public long ChannelId { get; set; }

        [Column("account_id")]
        [Description("Id account lưu trên hệ thống channel manager")]
        public long AccountId { get; set; }

        [Column("product_id")]
        [Description("Id product lưu trên hệ thống channel manager")]
        public long ProductId { get; set; }

        [Column("package_plan_id")]
        [Description("Id package plan lưu trên hệ thống channel manager")]
        public long PackagePlanId { get; set; }

        [Column("room_type_id")]
        [Description("Id room type lưu trên hệ thống channel manager")]
        public long RoomTypeId { get; set; }

        [Column("status")]
        [Description("Trạng thái hoạt động")]
        [MaxLength(1)]
        public string? Status { get; set; } = ActiveStatus.Active.ToEnumMemberString();

        public virtual ChannelRoomTypeEntity ChannelRoomType { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public virtual ChannelEntity Channel { get; set; }
        public virtual AccountEntity Account { get; set; }
        public virtual ProductEntity Product { get; set; }
        public virtual PackagePlanEntity PackagePlan { get; set; }
        public virtual RoomTypeEntity RoomType { get; set; }
    }
}
