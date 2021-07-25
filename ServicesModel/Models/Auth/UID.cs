using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Auth
{
    public class UID
    {
        public int id { get; set; }
        public string uid { get; set; }
        public DateTime updateDttm { get; set; }
        [ForeignKey("Auth")]
        public int id_user { get; set; }
        public virtual ServicesModel.Models.Auth.Auth Auth { get; set; }
    }
}
