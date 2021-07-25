using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Auth
{
    public class SendRegister
    {
        public string firstname { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string id { get; set; }
        public string lasname { get; set; }
        public string phone { get; set; }
        public bool single { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string site { get; set; }
        public int level0 { get; set; }
        public List<int> level1 { get; set; }
        public string uid { get; set; }
    }
}
