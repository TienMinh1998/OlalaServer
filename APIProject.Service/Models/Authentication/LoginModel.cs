/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 06/12/2021
 * Edit     : NOT YET
 * Content  : Model đăng nhập Trong APP, Get Token :
 * ----------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Service.Models.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
        public string DeviceID { get; set; }
    }


}
