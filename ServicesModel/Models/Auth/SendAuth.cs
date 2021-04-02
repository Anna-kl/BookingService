using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Auth
{
  public  class SendAuth
    {
        public int accountid { get; set; }
        public string token { get; set; }
        public bool isfilled { get; set; }
        public string avatar { get; set; }
        public string name { get; set; }
        public string role { set; get; }
    }
}
