using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Account
{
    public class CategoryAccount
    {
        public int id { get; set; }
        public int level0 {get;set;}
    public int level1 { get; set; }
        [ForeignKey("Account")]
        public int id_account { get; set; }
        public virtual Account Account { get; set; }
    }
}
