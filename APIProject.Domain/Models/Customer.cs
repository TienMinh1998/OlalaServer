/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 13/12/2021
 * Edit     : Thêm trường Email cho khách hàng
 * Content  : Model Customer dùng để Migration cơ sở dữ liệu
 * ----------------------------------*/


using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Customer : BaseModel
    {
        [StringLength(15)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string DeviceID { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public string CodeTax { get; set; } // mã số thuế
        public int CustomerTypeID { get; set; }
        public long Point { get; set; }
        public CustomerType CustomerType { get; set; } 
        public int? IsConfirmRole { get; set; }
        public int Status { get; set; } = SystemParam.ACTIVE;
        public ICollection<ReceiveAddress> ReceiveAddresses { get; set; }
        public DateTime? ExpireDateOTP { get; set; }
        public string OTP { get; set; }
        public int QtyOTP { get; set; }
        public int? IsConfirmOTP { get; set; }
    }
}
