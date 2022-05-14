using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class ProductStorage : BaseModel
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int? StorageID { get; set; }
        public Storage Storage { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Supplier { get; set; }
        public string LotNo { get; set; }

    }
}
