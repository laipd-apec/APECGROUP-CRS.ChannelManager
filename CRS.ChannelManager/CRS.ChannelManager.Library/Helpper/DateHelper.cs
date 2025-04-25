using Core.Helpper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Helpper
{
    public static class DateHelper
    {
        private static readonly DateTime genesisDate = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static int DateToNumber(DateTime dateTime)
        {
            return (dateTime - genesisDate).Days;
        }
    }
}
