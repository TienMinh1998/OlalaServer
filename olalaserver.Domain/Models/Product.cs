using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Product : BaseModel
    {
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        [StringLength(150)]
        public string Size { get; set; } // Kích cỡ
        [StringLength(150)]
        public string Origin { get; set; } // Nguồn gốc
        [StringLength(150)]
        public string Ingredient { get; set; } // Thành phần
        [StringLength(150)]
        public string Usage { get; set; } // Cách sử dụng
        [StringLength(150)]
        public string StorageTemperature { get; set; } // Nhiệt độ bảo quản
        public double NetWeight { get; set; }
        public int MinQuantityStorage { get; set; } // Số lượng tồn kho tối thiểu
        public string Unit { get; set; } // Nhóm hàng
        public int Status { get; set; } = SystemParam.ACTIVE;
        public int? Type {get; set; } // Loại sản phẩm hot , bán chạy , khuyến mãi
        public int? CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }
    }
}
