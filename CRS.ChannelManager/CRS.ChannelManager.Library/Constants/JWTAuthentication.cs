using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Constants
{
    public class JWTAuthentication
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string NameAdmin = "Quản trị";
            public const string Operate = "Operate";
            public const string NameOperate = "Vận hành";
            public const string Staff = "Staff";
            public const string NameStaff = "Nhân viên";

            public const string AdminOperate = "Admin,Operate";
            public const string AdminStaff = "Admin,Staff";
            public const string OperateStaff = "Operate,Staff";
            public const string AdminOperateStaff = "Admin,Operate,Staff";
        }

        public static class Users
        {
            public const string AccountAdmin = "admin";
            public const string PassWordAdmin = "123456aA@";
            public const string FullNameAdmin = "Quản trị hệ thống";

            public const string AccountOperate = "operate";
            public const string PassWordOperate = "123456aA@";
            public const string FullNameOperate = "Vận hành hệ thống";

            public const string AccountStaff = "staff";
            public const string PassWordStaff = "123456aA@";
            public const string FullNameStaff = "Người dùng hệ thống";
        }

    }
}
