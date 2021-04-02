
using Newtonsoft.Json;
using ServicesModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.Helpers.Responce.Responce;

namespace Application.Helpers.Responce
{
    public class ResponeRepository : IResponce
    {
        private ServicesContext db_base;
        public ResponeRepository(ServicesContext db)
        {
            db_base = db;
        }
     
   //public Account Change(Account old, Account newdata)
   //     {
   //         if (newdata.name != null)
   //         {
   //             old.name = newdata.name;
   //         }
   //         //if (newdata.phone != null)
   //         //{
   //         //    old.phone = newdata.phone;
   //         //}
   //         if (newdata.email != null)
   //         {
   //             old.email = newdata.email;
   //         }
   //         if (newdata.address != null)
   //         {
   //             old.address = newdata.address;
   //         }
   //         if (newdata.coordinate != null)
   //         {
   //             old.coordinate = newdata.coordinate;
   //         }
   //         if (newdata.describe != null)
   //         {
   //             old.describe = newdata.describe;
   //         }
          
   //         old.lastvisit = DateTime.Now;
   //         return old;
   //     }
     
        public Responce.Answer Return_Responce(System.Net.HttpStatusCode code, object data, string error)
        {
            state st = new state
            {
                message = error,
                code = code
            };
          
               
                Responce.Answer asn = new Responce.Answer
                {
                  status=st,  
                    responce = data
                };
                return asn;
            
            

        }
        //public bool CheckAccountCustomer(Customer account)
        //{
            
        //    if (account.email == null)
        //    {
        //        return false;
        //    }
        //    if (account.name == null)
        //    {
        //        return false;
        //    }
        //    if (account.phone == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
     
    }

}
