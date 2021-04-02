using ServicesModel.Models;
using ServicesModel.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.BookingServices.Service.Helpers
{
    public class ServiceHelpers
    {
        public StaffService LoadService(SendServices send, int id_category )
        {
            StaffService st = new StaffService();
            st.category = id_category;
            st.descride = send.descride;
            st.price = send.price;
            st.minutes = send.minutes;
            st.name = send.name;
            return st;
        }
    }
}
