using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Geo
{
   public class Coordinate
    {
        public int id { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
          [ForeignKey("Account")]
        public int account_id { get; set; }
        public virtual ServicesModel.Models.Account.Account Account { get; set; }
    }

   
}
