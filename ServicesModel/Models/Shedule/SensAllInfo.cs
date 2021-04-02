using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Shedule
{
   public class SendAllInfo
    {
        public int id { get; set; }
        public string services_name { get; set; }
        public DateTime start_dttm { get; set; }
        public DateTime end_dttm { get; set; }
        public string name_client { get; set; }
        public string comment_client { get; set; }
        public string comment_service { get; set; }
        public bool iscanceled { get; set; }
        public int client_id { get; set; }
        public float price { get; set; }

    }
}
