using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Auth
{
   public class Confirm
    {
        public int id { get; set; }
        [ForeignKey("Auth")]
        public int user_id { get; set; }
        public string code { get; set; }
        public DateTime send { get; set; }
        public virtual Auth auth { get; set; }
        
    }
}
