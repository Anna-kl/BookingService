using ServicesModel.Models.Services;
using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models
{
   public class StaffService
    {
        [Key]
        public int id { get; set; }
        public float price { get; set; }
        public string name { get; set; }
        public string descride { get; set; }
        public int minutes { get; set; }
        [ForeignKey("EmployeeOwner")]
        public int account_id { get; set; }
        public int category { get; set; }
        public virtual EmployeeOwner EmployeeOwner { get; set; }
       
    }
   
}
