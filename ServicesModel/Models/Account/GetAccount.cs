using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Account
{
  public  class GetAccount
    {
        public int id { get; set; }
        public string category { get; set; }
        public string address { get; set; }
      
        public int rating { get; set; }
      public double distinct { get; set; }
     public string name { get; set; }
      
    }
}
