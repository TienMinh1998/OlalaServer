using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Users
{
   public class CreateUserModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
    }
}
