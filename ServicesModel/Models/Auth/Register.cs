
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServicesModel.Models.Auth
{
    /// <summary>
    /// Модель для регистрации пльзователя
    /// </summary>
   public class Register
    {
        /// <summary>
        /// телефон или email (телефон в формате 9998887766)
        /// </summary>
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        /// <summary>
        /// client, staff, owner
        /// </summary>
        [Required]
        public string role { get; set; }
        /// <summary>
        /// код телефона (уникальный номер устройства)
        /// </summary>
        public string uid { get; set; }
    }
    public class Password
    {
        public string email { get; set; }
    }
}
