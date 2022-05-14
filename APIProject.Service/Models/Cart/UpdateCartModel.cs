
/*-----------------------------------
 * Author   : NGuyễn Viết Minh Tiến
 * DateTime : 29/12/2021
 * Edit     : chưa chỉnh sửa
 * Content  : AddCartModel  
 * ----------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Cart
{
   public class UpdateCartModel
    {
        public int CartID { get; set; }
        public int Quantity { get; set; }
    }
}
