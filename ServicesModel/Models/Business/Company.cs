using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ServicesModel.Models.Business
{
   public class Company
    {
        public int account_id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public int rating { get; set; }
        public string desc { get; set; }
        public int category { get; set; }
    }
}
