using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Password
{
   public class ChangePasswordOTPModel
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
    public class ChangePasswordModel
    {
        public string Password { get; set; }
    }
}
