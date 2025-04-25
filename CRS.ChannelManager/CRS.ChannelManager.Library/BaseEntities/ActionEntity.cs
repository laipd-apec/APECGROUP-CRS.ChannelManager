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
    public class ActionEntity : EntityBase
    {
        [Column("CODE"), MaxLength(50), Description("Mã hành động"), Required]
        public string Code { get; set; }


        [Column("NAME"), MaxLength(500), Description("Tên của hành động"), Required]
        public string Name { get; set; }
    }
}
