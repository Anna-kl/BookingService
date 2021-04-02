using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Shedule
{
   public class SendTime
    {
        public int services_id { get; set; }
        public DateTime start { get; set; }
        public int dayof { get; set; }
        public int client_id { get; set; }
        public string phone { get; set; }
        public  string client_name { get;set;}
        public string services_comment { get; set; }
    }
}
