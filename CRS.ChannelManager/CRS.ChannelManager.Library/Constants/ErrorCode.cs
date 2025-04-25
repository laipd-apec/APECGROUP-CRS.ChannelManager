using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Constants
{
    public enum ErrorCode
    {
        SQL_BASE_ERROR = 202,

        SERVICE_BASE_ERROR = 503,

        SERVICE_CREATE_ERROR = 550,
        SERVICE_UPDATE_ERROR = 551,
        SERVICE_DELETE_ERROR = 552,
        SERVICE_NOT_EXISTS = 553,
        SERVICE_ALREADY_EXISTS = 554,

        HTTP_REQUEST_ERROR = 500,


    }
}
