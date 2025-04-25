using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class ValueDto
    {
        public class ValueRequestDto
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public string Value { get; set; }
        }
    }
}
