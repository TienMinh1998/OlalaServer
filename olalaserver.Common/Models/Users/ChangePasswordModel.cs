using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Users
{
    public class ChangePasswordWebModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
