using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Shedule
{
   public class ConctereDay
    {
        [Key]
        public int id { get; set; }
        public bool iscanceled { get; set; }
        public int services_id { get; set; }
        public DateTime dttm_start { get; set; }
        public DateTime dttm_end { get; set; }
        public bool is_complete { get; set; }
        public string services_comment { get; set; }
        public float price { get; set; }
        public string comment { get; set; }
        [ForeignKey("DayOfWork")]
        public int daysof { get; set; }
        public int client_id { get; set; }
        public virtual DayOfWork DayOfWork { get; set; }
    }
}
