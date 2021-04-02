using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServicesModel.Models.Auth
{
  public  class Change_Password
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public DateTime dttm { get; set; }
        [DefaultValue("false")]
        public bool state { get; set; }
        public string password { get; set; }
    }
}
