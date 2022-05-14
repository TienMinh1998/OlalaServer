using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Users
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }
    public class UserInfoModel
    {
        public int ID { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int Status { get; set; }
        public List<int> ListPermission { get; set; }
    }
}
