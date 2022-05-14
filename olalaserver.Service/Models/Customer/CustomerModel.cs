/*-----------------------------------
 * Author   : NGuyễn Viết Minh Tiến
 * DateTime : 27/11/2021
 * Edit     : - THêm mật khẩu lần 2
 * Content  : Model Customer 
 * ----------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models
{
    public class CustomerModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DeviceID { get; set; }
        public int Type { get; set; }
        public int Point { get; set; }
        public string Token { get; set; }
    }
  
}
