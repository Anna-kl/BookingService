using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Shedule
{
   public class SheduleSend
    {
       public int dayof { get; set; }
        public List<DateTime> dttm { get; set; }
    }

    public class DateSend
    {
        public DateTime date { get; set; }
    }
}
