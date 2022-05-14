using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class OrderHistory : BaseModel
    {
        public int Status { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}
