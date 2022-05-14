using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.ProductStorage
{
    public class ProductStorageModel
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; } // Nhóm hàng
        public string Size { get; set; } // Kích cỡ
        public string Origin { get; set; } // Nguồn gốc
        public string Storage { get; set; }
        public int Quantity { get; set; }
        public double NetWeight { get; set; }
        public double TotalWeight { get; set; }
        public double MinQuantityStorage { get; set; } // Lượng tồn kho tối thiểu
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string LotNo { get; set; }
        public string Supplier { get; set; }
    }
    public class ProductStorageByProductModel
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int TotalQuantity { get; set; }
        public double NetWeight { get; set; }
        public double TotalWeight { get; set; }
        public List<StorageQuantityModel> ListStorageQuantity { get; set; }
    }
    public class StorageQuantityModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double TotalWeight { get; set; }
    }
    public class ProductStorageDetailModel
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public string Supplier { get; set; }
        public string Unit { get; set; } // Nhóm hàng
        public string Storage { get; set; }
        public int Quantity { get; set; }
        public double NetWeight { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
    public class ProductStorageHistoryModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; } // Loại nhập/xuất kho
        public int Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
