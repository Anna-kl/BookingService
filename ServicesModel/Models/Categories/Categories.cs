using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesModel.Models.Categories
{
    public class Category
    {
        public int id { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public int parent { get; set; }
       
    }
}
