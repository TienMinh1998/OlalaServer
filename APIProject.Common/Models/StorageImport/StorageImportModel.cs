using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.StorageImport
{
    public class StorageImportModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Storage { get; set; }
        public DateTime ImportDate { get; set; }
        public double TotalWeight { get; set; }
        public long TotalPrice { get; set; }
    }

    public class StorageImportDetailModel : StorageImportModel
    {
        public List<StorageImportProductModel> StorageImportProducts { get; set; }
    }
    public class StorageImportProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Supplier { get; set; }
        public string Size { get; set; }
        public string Origin { get; set; }
        public string LotNo { get; set; }
        public string Unit { get; set; } // Nhóm hàng
        public int Quantity { get; set; }
        public double NetWeight { get; set; }
        public double MinQuantityStorage { get; set; }
        public long Price { get; set; }
        public double TotalWeight { get; set; }
        public long TotalPrice { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Note { get; set; }
    }
    public class CreateStorageImportModel
    {
        public int StorageID { get; set; }
        public string ImportDate { get; set; }
        public List<CreateStorageImportProductModel> StorageImportProducts { get; set; }
    }
    public class CreateStorageImportProductModel
    {
        public int ProductID { get; set; }
        public string Supplier { get; set; }
        public string LotNo { get; set; } 
        public int Quantity { get; set; }
        public long Price { get; set; }
        public string ManufactureDate { get; set; }
        public string ExpiredDate { get; set; }
        public string Note { get; set; }
    }
}
