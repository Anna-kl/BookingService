using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Auth
{
    public class Token
    {
        [Key]
        public long id { get; set; }
     [ForeignKey("Auth")]
        public int user_id { get; set; }
        public string access { get; set; }
        public string refresh { get; set; }
        public DateTime access_generate { get; set; }
        public DateTime refresh_generate { get; set; }
        public DateTime access_expire { get; set; }
        public DateTime refresh_expire { get; set; }
        public virtual Auth Auth { get; set; }
    }
}
