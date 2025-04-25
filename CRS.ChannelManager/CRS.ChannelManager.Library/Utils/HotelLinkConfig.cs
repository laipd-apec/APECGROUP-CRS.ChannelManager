using CRS.ChannelManager.Library.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Utils
{
    public class HotelLinkConfig
    {
        public string ApiUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HotelAuthenticationChannelKey { get; set; }

        public const string API_AUTH_URL = "v2/external/oAuth/token";
        public const string API_GETBOOKING_URL = "v2/external/pms/getBookings";
        public const string API_GETRATEPLAN_URL = "v2/external/pms/getRatePlans";
        public const string API_READ_NOTIFICATION_URL = "v2/external/pms/readNotification";
    }
}
