using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class OrderDetail : BaseModel
    {

        public long Price { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public long SumPrice { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}
