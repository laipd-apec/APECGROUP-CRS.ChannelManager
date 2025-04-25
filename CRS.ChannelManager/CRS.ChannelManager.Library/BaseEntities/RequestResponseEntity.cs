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
    public class RequestResponseLogEntity : EntityBase
    {
        [Column("URL"), MaxLength(200), Description("Đường dẫn API"), Required]
        public string Url { get; set; }


        [Column("METHOD"), MaxLength(200), Description("Phương thức"), Required]
        public string Method { get; set; }


        [Column("HEADER"), MaxLength(int.MaxValue), Description("Cấu hình header đẩy lên"), Required]
        public string Header { get; set; }


        [Column("REQUEST"), MaxLength(int.MaxValue), Description("Nội dung gửi lên"), Required]
        public string Request { get; set; }


        [Column("RESPONSE"), MaxLength(int.MaxValue), Description("Nội dung trả về"), Required]
        public string Response { get; set; }

    }
}
