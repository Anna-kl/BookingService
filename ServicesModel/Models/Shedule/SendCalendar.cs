using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Shedule
{
  public  class SendCalendar
    {
        public int id_staff { get; set; }
        public List<string> days { get; set; }
    }
}
