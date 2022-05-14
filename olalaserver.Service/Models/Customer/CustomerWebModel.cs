

/*----------------------------------------------*
 * Author   : NGuyễn Viết Minh Tiến             *
 * DateTime : 15/12/2021                        *
 * Edit     : Chưa chỉnh sửa                    *
 * Content  : Model trả ra danh sách khách hàng *
 * ---------------------------------------------*/


using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Customer
{
   public class CustomerWebModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CustomerType { get; set; }
        public int IsConfirm { get; set; }
        public int Email { get; set; }
        public int  Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
