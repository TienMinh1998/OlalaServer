using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.StorageExport
{
    public class StorageExportModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Storage { get; set; }
        public DateTime ExportDate { get; set; }
        public long TotalPrice { get; set; }
        public double TotalWeight { get; set; }
        public string Province { get; set; }
        public string Condition { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public string Customer { get; set; }
        public string ReceiverName { get; set; }
        public string NumberCar { get; set; } // Số xe nhận
    }

    public class StorageExportDetailModel : StorageExportModel
    {
        public List<StorageExportProductModel> StorageExportProducts { get; set; }
    }
    public class StorageExportProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Supplier { get; set; }
        public string LotNo { get; set; }
        public string Unit { get; set; } // Nhóm hàng
        public int Quantity { get; set; }
        public double NetWeight { get; set; }
        public long Price { get; set; }
        public double TotalWeight { get; set; }
        public long TotalPrice { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
    public class CreateStorageExportModel
    {
        public string ExportDate { get; set; }
        public string Condition { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public string Customer { get; set; }
        public string ReceiverName { get; set; }
        public int ProvinceID { get; set; }
        public string NumberCar { get; set; } // Số xe nhận
        public List<CreateStorageExportProductModel> StorageExportProducts { get; set; }
    }
    public class CreateStorageExportOrderModel
    {
        public DateTime ExportDate { get; set; }
        public string Reason { get; set; }
        public string Customer { get; set; }
        public string ReceiverName { get; set; }
        public int ProvinceID { get; set; }
        public List<StorageExportProductDetailModel> StorageExportProducts { get; set; }
    }

    public class CreateStorageExportProductModel
    {
        public int ProductStorageID { get; set; }
        public int StorageID { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
    public class StorageExportProductDetailModel : CreateStorageExportProductModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Supplier { get; set; }
        public string LotNo { get; set; }
        public string Unit { get; set; } // Nhóm hàng
        public double NetWeight { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
