using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Cart : BaseModel
    {
        public long Price { get; set; }
        public int Quantity { get; set; }
        public long SumPrice { get; set; }
        public int ProductItemID { get; set; }
        public ProductItem ProductItem { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int Type { get; set; }
    }
}
