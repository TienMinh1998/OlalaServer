using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class User : BaseModel
    {
        [StringLength(100)]
        public string Username { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        public int RoleID { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int Status { get; set; }
        public Role role { get; set; }
    }
}
 