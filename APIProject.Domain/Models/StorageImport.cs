using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class StorageImport : BaseModel
    {
        [StringLength(50)]
        public string Code { get; set; }
        public DateTime ImportDate { get; set; }
        public int StorageID { get; set; }
        public Storage Storage { get; set; }
        public long TotalPrice { get; set; }
        public double TotalWeight { get; set; }
        public ICollection<StorageImportDetail> StorageImportDetails { get; set; }
    }
}
