using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.ChannelManager.Library.Base;

namespace CRS.ChannelManager.Library.BaseEntities
{
    public class MessageEntity : EntityBase
    {
        [Column("CODE_LANGUAGE"), MaxLength(100), Description("Mã ngôn ngữ"), Required]
        public string CodeLanguage { get; set; }


        [Column("CODE"), MaxLength(100), Description("Mã thông báo"), Required]
        public string Code { get; set; }


        [Column("MESSAGE"), MaxLength(500), Description("Nội dung thông báo"), Required]
        public string Message { get; set; }

    }
}
