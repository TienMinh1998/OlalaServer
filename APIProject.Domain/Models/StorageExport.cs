using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class StorageExport : BaseModel
    {
        [StringLength(50)]
        public string Code { get; set; }
        public string Customer { get; set; }
        public string ReceiverName { get; set; }
        public string NumberCar { get; set; } // Số xe nhận
        public DateTime ExportDate { get; set; }
        public int StorageID { get; set; }
        public Storage Storage { get; set; }
        public int ProvinceID { get; set; }
        public Province Province { get; set; }
        public string Condition { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public long TotalPrice { get; set; }
        public double TotalWeight { get; set; }
        public ICollection<StorageExportDetail> StorageExportDetails { get; set; }
    }
}
