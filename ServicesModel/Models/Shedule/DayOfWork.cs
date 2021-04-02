using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServicesModel.Models.Shedule
{
    public class DayOfWork
    {
        public int id { get; set; }
        public DateTime dttmStart { get; set; }
        public DateTime dttmEnd { get; set; }
        [ForeignKey("EmployeeOwner")]
        public int accountId { get; set; }
        public virtual EmployeeOwner EmployeeOwner { get; set; }
    }
    //public class SendDaysWork{
    //    public DayOfWork day { get; set; }
    //    public List<ConctereDay> concrete { get; set; }
    //    }
    public class SendDaysWork
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int id_day { get; set; }
        public List<SendAllInfo> send { get; set; }

    }
    public class concreteSend
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string nameservices { get; set; }
        public string nameclient { get; set; }
        public float price { get; set; }
    }

    }
