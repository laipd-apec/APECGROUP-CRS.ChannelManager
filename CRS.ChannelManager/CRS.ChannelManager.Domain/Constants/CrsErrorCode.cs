using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Constants
{
    public static class CrsErrorCode
    {
        public static readonly int Success = 200;
        public static readonly string SuccessMsg = "Success";

        public static readonly int Anauthorized = 401;
        public static readonly string AnauthorizedMsg = "Unauthorized";
    }
}
