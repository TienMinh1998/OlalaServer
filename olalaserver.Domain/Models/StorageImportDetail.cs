using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class StorageImportDetail : BaseModel
    {
        public int StorageImportID { get; set; }
        public StorageImport StorageImport { get; set; }
        public int ProductStorageID { get; set; }
        public ProductStorage ProductStorage { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public double TotalWeight { get; set; }
        public long TotalPrice { get; set; }
        public string Note { get; set; }


    }
}
