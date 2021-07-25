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
            /// <summary>
            /// Json объекта ответа сервера
            /// </summary>
            public object responce { get; set; }
        }

        /// <summary>
        /// Статус ответа
        /// </summary>
        public class state
        {
            /// <summary>
            /// HttpStatus code
            /// </summary>
            public System.Net.HttpStatusCode code { get; set; }
            /// <summary>
            /// Сообщение об ошибке
            /// </summary>
            public string message { get; set; }
        }
    }

}
