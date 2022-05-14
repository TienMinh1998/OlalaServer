using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Authentication
{
   public class RegisterModel
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int CustomerTypeID { get; set; }
        public string CodeTax { get; set; }
        public string DeviceID { get; set; }
        public string Avatar { get; set; }
    }
}
