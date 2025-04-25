using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.HanderException
{
    public class ErrorCode
    {
        public static ErrorDto DUPLICATE_CODE = new ErrorDto(1000, "duplicate code");
        public static ErrorDto UNAUTHORIZED_CODE = new ErrorDto(401, "Unauthorized");
        public static ErrorDto NOT_FOUND_USER_CODE = new ErrorDto(401, "User not exitst");
    }
}
