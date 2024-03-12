using SV20T1020293.DataLayers.SQLServer;
using SV20T1020293.DataLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV20T1020293.DomainModels;

namespace SV20T1020293.BusinessLayers
{
   
   public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeUserAccountDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static UserAccountService()
        {
            employeeUserAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }

        public static UserAccount Authorize(string userName, string password)
        {
            //TODO: Kiểm tra thông tin đăng nhập của Employee
            return employeeUserAccountDB.Authorize(userName, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            //TODO: Thay đổi mật khẩu của Employee
            return employeeUserAccountDB.ChangePassword(userName, oldPassword, newPassword);
        }

    }
}
