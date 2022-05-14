using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class ProductItem : BaseModel
    {
        public long Price { get; set; }
        public long OriginalPrice { get; set; }
        public int CustomerTypeID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
