using ServicesModel.Models.Account;
using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.BookingServices.Account.AccountServices
{
    public interface IAccount
    {
        bool CheckFilledStaff(EmployeeOwner account);
         ServicesModel.Models.Account.Account Changeacount(ServicesModel.Models.Account.SendAccount account_main,
            ServicesModel.Models.Account.Account account);
          Task<ServicesModel.Models.Auth.SendAuth> ReturnAuth(int user_id, string role);
        bool CheckFilled(ServicesModel.Models.Account.Account account);
        Task<SendAccount> GetData(ServicesModel.Models.Account.Account account_main);
    }
}
