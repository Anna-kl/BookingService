
using ServicesModel.Models.Auth;
using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers.Authentication
{
 public   interface IAutentication
    {
     //   public bool CheckFilledStaff(EmployeeOwner account);
        public  Task<bool> ChangePassword(Auth auth);
        public Task<string> Confirm(int user, string email);
        //string BuildToken(string username, string password);
        //bool Check_Registration(string user);
        string GetPrincipalFromExpiredToken(string token);
        //Token Generate_Tokens(string user, string password);
        Task Insert_Tokens(Token token);
        //Task Write_Users(string user, string passworde);
        //bool Refresh_access(Token token);
        //Token Find_tokens(string user, string password);
        string GenerateRefreshToken();
        public  Task<bool> ConfirmStaff(string link, string email);
        public Task<bool> SendLogin(string email, string password);
        Token Return_tokens(string access, string refresh);
        //Task<string> Order_Change_Password(string user);
        Token Generate_Tokens(int user_id, string roles);
        string Check_Tokens(string access, string refresh);
        //Task<Token> Refresh_Tokens(string refresh);
        //Task<string> CheckEmail(string link);
        //Task GenerateCheck(string user);
    }
}
