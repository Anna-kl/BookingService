using Microsoft.EntityFrameworkCore;
using ServicesModel.Context;
using ServicesModel.Models.Account;
using ServicesModel.Models.Auth;
using ServicesModel.Models.Clients;
using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.BookingServices.Account.AccountServices
{
    public class AccountRepository:IAccount
    {
        private ServicesContext _context;
        public AccountRepository(ServicesContext db)
        {
            _context = db;
        }
        public bool CheckFilled(ServicesModel.Models.Account.Account account)
        {
         
            if (account.email == null)
            {
                return false;
            }
            if (account.address == null)
            {
                return false;
            }
            if (account.email == null)
            {
                return false;
            }
            if (account.phone == null)
            {
                return false;
            }
            return true;

        }
        public bool CheckFilledStaff(EmployeeOwner account)
        {

            if (account.email == null)
            {
                return false;
            }
           
            if (account.firstname == null)
            {
                return false;
            }
            if (account.phone == null)
            {
                return false;
            }
            return true;

        }
        public bool CheckFilledClient(Client account)
        {

            if (account.email == null)
            {
                return false;
            }

            if (account.name == null)
            {
                return false;
            }
            if (account.phone == null)
            {
                return false;
            }
            return true;

        }
        public async Task<SendAccount> GetData(ServicesModel.Models.Account.Account account_main)
        {
            SendAccount account = new SendAccount();
            account.name = account_main.name;
            account.phone = account_main.phone;
            account.address = account_main.address;
            account.email = account_main.email;
            account.update = account_main.update;
            var category = await _context.categoryAccounts.Where(x => x.id_account == account_main.id).FirstOrDefaultAsync();
            if (category != null)
            {
                account.level0 = category.level0;
                account.level1 = category.level1;
            }
            return account;
        }
        public async Task<ServicesModel.Models.Auth.SendAuth> ReturnAuth(int user_id,  string role)
        {
            SendAuth send = null;
            if (role == "owner")
            {
                var account = await _context.Accounts.Where(x => x.id_user == user_id).FirstOrDefaultAsync();
                send = new SendAuth
                {
                    accountid = account.id,
                    isfilled = CheckFilled(account),
                    name = account.name,
                    role = role
                };
            }
            else if (role=="staff")
            {
                var account = await _context.EmployeeOwners.Where(x => x.id_user == user_id).FirstOrDefaultAsync();
                send = new SendAuth
                {
                    accountid = account.id,
                    isfilled = CheckFilledStaff(account),
                    name = account.firstname,
                    role = role
                };
            } else
            {
                var account = await _context.Clients.Where(x => x.id_user == user_id).FirstOrDefaultAsync();
                send = new SendAuth
                {
                    accountid = account.id,
                    isfilled = CheckFilledClient(account),
                    name = account.name,

                };
            }
            var token = await _context.Tokens.Where(x => x.user_id == user_id).FirstOrDefaultAsync();
            send.token = token.access;
            return send;
        }
        public ServicesModel.Models.Account.Account Changeacount(ServicesModel.Models.Account.SendAccount account_main,
            ServicesModel.Models.Account.Account account)
        {
            account.name = account_main.name;
            account.phone = account_main.phone;
            account.address = account_main.address;
            account.email = account_main.email;
            account.update = DateTime.Now;
            return account;

        }
    }
}
