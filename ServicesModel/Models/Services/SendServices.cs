using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Services
{
  public  class SendServices
    {
        public int id { get; set; }
        public float price { get; set; }
        public string name { get; set; }
        public string descride { get; set; }
        public int minutes { get; set; }
   
        public string category { get; set; }
   
    }
}
