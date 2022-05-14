using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Product
{
    public class ProductAppModel
    {
        public int ID { get; set; }
        public long? Price { get; set; }
        public long? OriginalPrice { get; set; }
        public int? Type { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
    public class ProductDetailAppModel : ProductBaseModel
    {
        public int ID { get; set; }
        public long? Price { get; set; }
        public long? OriginalPrice { get; set; }
        public List<ProductAppModel> ListProductRelative { get; set; }
    }
}
