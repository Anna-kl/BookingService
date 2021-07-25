using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Auth
{
  public  class SMSCode
    {
        public int id { get; set; }
        public string phone { get; set; }
        public DateTime dttm_add { get; set; }
        public string code { get; set; }
        public bool status { get; set; }
    }

    public class CheckCode
    {
        public string phone { get; set; }
        public string code { get; set; } 
    }
}
