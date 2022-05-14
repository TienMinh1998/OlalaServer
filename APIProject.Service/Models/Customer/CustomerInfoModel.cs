
/*-------------------------------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 20/12/2021
 * Edit     : Chưa
 * Content  : Model Thông Tin APP
 * ------------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Customer
{
   public class CustomerInfoModel 
    {
 
        public string Phone { get; set; }
        public string Name { get; set; }
        //public string Password { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string CodeTax { get; set; }            
        public int? CustomerTypeID { get; set; }
        public int Status { get; set; }
    }
}
