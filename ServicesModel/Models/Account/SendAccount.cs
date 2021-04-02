using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServicesModel.Models.Account
{
   public class SendAccount
    {
        public int level0 { get; set; }
        public int level1 { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
       public DateTime update { get; set; }
    }
   
}
