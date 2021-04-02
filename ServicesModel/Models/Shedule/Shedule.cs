using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Staff
{
  public  class Shedule
    {
        [Key]
        public int id { get; set; }
        public DateTime dttm_start { get; set; }
        public DateTime dttm_end { get; set; }
        [ForeignKey("Service")]
        public int service_id { get; set; }
        public bool complete { get; set; }
        public virtual StaffService Service { get; set; }
    }
}
