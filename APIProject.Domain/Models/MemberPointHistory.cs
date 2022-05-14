using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class MemberPointHistory : BaseModel
    {
        public long Balance { get; set; }
        public long Point { get; set; }
        public int Type { get; set; }
        public int TypeAdd { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int? OrderID { get; set; }
        public Order Order { get; set; }
        public string Description { get; set; }
    }
}
