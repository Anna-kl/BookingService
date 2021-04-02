using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers.Responce
{
    public class Responce
    {
        public class Answer
        {
            public state status { get; set; }
            public object responce { get; set; }
        }
        public class state
        {
            public System.Net.HttpStatusCode code { get; set; }
            public string message { get; set; }
        }
    }

}
