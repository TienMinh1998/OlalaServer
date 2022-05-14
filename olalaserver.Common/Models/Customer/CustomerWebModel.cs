using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Customer
{
    public class CustomerWebModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public int? IsConfirm { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
