using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ServicesModel.Models.Auth;
namespace ServicesModel.Models.Account
{
 public  class Account
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [ForeignKey("Auth")]
        public int id_user { get; set; }
        public DateTime update { get; set; }
        public virtual ServicesModel.Models.Auth.Auth Auth { get; set; }
    }
}
