using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Customer
{
    public class ChangeCustomerInfoModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public int? Gender { get; set; }
    }
}
