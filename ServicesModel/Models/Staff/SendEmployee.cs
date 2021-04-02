using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServicesModel.Models.Staff
{
  public  class SendEmployee
    {
      public int id { get; set; }
  public string  name { get; set; }
        public DateTime lastvisit { get; set; }
  public string  email { get; set; }
  public string  role { get; set; }
        public object avatar { get; set; }
        public SendEmployee(EmployeeOwner owner)
        {
            this.email = owner.email;
            this.id = owner.id;
            this.name = owner.firstname;
            

        }
    }
}
