using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Images
{
    public class UserpicStaff
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime dttmadd { get; set; }
        public string path { get; set; }
        [ForeignKey("EmployeeOwner")]
        public int account_id { get; set; }
        public virtual EmployeeOwner EmployeeOwner { get; set; }
    }
}
