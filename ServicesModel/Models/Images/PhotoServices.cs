using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Images
{
    public class PhotoServices
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        [ForeignKey("StaffService")]
        public int id_service { get; set; }
        public DateTime dttmadd { get; set; }
        public virtual StaffService StaffService { get; set; }
    }
}
