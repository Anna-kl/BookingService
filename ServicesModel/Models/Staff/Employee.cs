using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Staff
{
    public class EmployeeOwner
    {
        [Key]
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        [Phone]
        public string phone { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string link { get; set; }
        public DateTime date_add { get; set; }
        public DateTime birthday { get; set; }
        public string position { get; set; }
        public int id_owner { get; set; }
        public bool accepted { get; set; }
        [ForeignKey("Auth")]
        public int id_user { get; set; }
        public virtual ServicesModel.Models.Auth.Auth Auth { get; set; }
    }


    public class SendEmployeeOwner
    {
    
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        [Phone]
        public string phone { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string link { get; set; }
        public DateTime date_add { get; set; }
        public DateTime birthday { get; set; }
        public string position { get; set; }
        public int id_owner { get; set; }
        public bool accepted { get; set; }
       
        public int id_user { get; set; }
        public string work { get; set; }
        public SendEmployeeOwner(EmployeeOwner emp)
        {
            email = emp.email; phone = emp.phone; lastname = emp.lastname; firstname = emp.firstname;
            middlename = emp.middlename; birthday = emp.birthday;
        }
    }
}
