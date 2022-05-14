using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class ProductStorageHistory : BaseModel
    {
        public int ProductStorageID { get; set; }
        public ProductStorage ProductStorage { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int Balance { get; set; } // SL sau khi cộng/trừ
        public int Type { get; set; } // Loại nhập kho/xuất kho
        public string Code { get; set; }
    }
}
