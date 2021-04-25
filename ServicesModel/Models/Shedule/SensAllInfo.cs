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

    public class Sendbusiness
    {
        public float price { get; set; }
        public int id { get; set; }
        public string services_name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string resource { get; set; }
        public string html { get; set; }
public int account_id { get; set; }

    }
}
