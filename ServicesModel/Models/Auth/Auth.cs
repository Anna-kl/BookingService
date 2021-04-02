
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServicesModel.Models.Auth
{
   public class Auth
    {
        [Key]
        public int id { get; set; }
      
        [DataType(DataType.EmailAddress)]
      
        public string email { get; set; }
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.PhoneNumber)]
       public string Phone { get; set; }
        public DateTime data_add { get; set; }
        public bool is_active { get; set; }
        [Required]
        public string role { get; set; }
        public DateTime last_visit { get; set; }
        [DefaultValue("false")]
        public bool is_confirm { get; set; }
        public bool EmailConfirmed { get; set; }
    }
 
}
