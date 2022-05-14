using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class StorageExportDetail : BaseModel
    {
        public int StorageExportID { get; set; }
        public StorageExport StorageExport { get; set; }
        public int ProductStorageID { get; set; }
        public ProductStorage ProductStorage { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public double TotalWeight { get; set; }
        public long TotalPrice { get; set; }
    }
}
