using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ServicesModel.Models.Auth;
namespace ServicesModel.Models.Clients
{
   public class Client
    {
        public int id { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        public DateTime update_date { get; set; }
        public string desc { get; set; }
        [ForeignKey("Auth")]
        public int id_user { get; set; }
      
        public virtual ServicesModel.Models.Auth.Auth Auth { get; set; }

    }
}
