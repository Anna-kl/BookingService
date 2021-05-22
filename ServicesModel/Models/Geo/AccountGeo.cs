using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Geo
{
   public class AccountGeo
    {
        public int category_id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class AccountGeoSend
    {
        public string name { get; set; }
        public double price { get; set; }
        public double distinct { get; set; }
        public int rating { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public FileStreamResult icon { get; set; }
    }
}
