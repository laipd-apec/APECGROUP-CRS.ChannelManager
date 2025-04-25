using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.HanderException
{
    
    public class ReturnStatusDto
    {
        public int result_code { get; set; }
        public int status { get; set; }
        public object errors { get; set; }
    }
}
