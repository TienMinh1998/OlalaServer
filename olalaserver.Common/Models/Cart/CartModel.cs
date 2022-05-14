using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Cart
{
    public class CartModel
    {
        public int  ID { get; set; }
        public int IsActice { get; set; }
        public DateTime CreateDate { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public long SumPrice { get; set; }
        public int ProductItemID { get; set; }
        public int CustomerID { get; set; }
        public int Type { get; set; }
    }
    public class CartModelDetail 
    {
        public int CartID { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public long SumPrice { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string  Unit { get; set; }
  
    }
}
