using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Images
{
    public class Userpic
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime dttmadd { get; set; }
        public string path { get; set; }
        [ForeignKey("Account")]
        public long account_id { get; set; }
    }
}
