using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Product
{
    public class ProductBaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
        public string Unit { get; set; }
        public string Size { get; set; }
        public double NetWeight { get; set; }
        public double MinQuantityStorage { get; set; }
        public string Origin { get; set; }
        public string StorageTemperature { get; set; }
        public string Ingredient { get; set; }
        public string Usage { get; set; }
        public int? Type { get; set; }
        public int? CategoryID { get; set; }
        public List<string> ListImage { get; set; }
        public string Description { get; set; }
    }
    public class ProductWebModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; } // Nhóm hàng
        public int? Type { get; set; } // Loại sản phẩm Hot/Bán Chạy/Khuyễn mãi
        public int Status { get; set; }
        public int Quantity { get; set; }
        public double NetWeight { get; set; }
        public double MinQuantityStorage { get; set; } // Lượng tồn kho tối thiểu
        public string Size { get; set; }
        public string Origin { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class CreateProductModel : ProductBaseModel
    {
        public int isNotify { get; set; }
        public List<ProductItemModel> ListProductItem { get; set; }

    }
    public class ProductDetailWebModel : CreateProductModel
    {
        public int ID { get; set; }
    }
    public class UpdateProductModel : ProductBaseModel
    {
        public int ID { get; set; }
        public List<ProductItemModel> ListProductItemCreate { get; set; }
        public List<ProductItemModel> ListProductItemUpdate { get; set; }
        public List<ProductItemModel> ListProductItemDelete { get; set; }
    }
    public class ProductItemModel
    {
        public int ID { get; set; }
        public int CustomerType { get; set; }
        public long Price { get; set; }
        public long? OriginalPrice { get; set; }
    }

}
