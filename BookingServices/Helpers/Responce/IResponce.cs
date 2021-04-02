
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers.Responce
{
    public interface IResponce
    {

      //  Account Change(Account old, Account newdata);
        //Customer Change(Customer old, Customer newdata);
        //bool CheckAccountCustomer(Customer account);
    //    bool CheckAccountContractor(Account account, string role);
        Responce.Answer Return_Responce(System.Net.HttpStatusCode code, object data, string error);
        //Responce.Error Return_Error(string error_string);
      
    }

}
