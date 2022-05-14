using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class ReceiveAddress :BaseModel
    {
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        public string Address { get; set; }
        public int IsDefault { get; set; }
        public int ProvinceID { get; set; }
        public Province Province { get; set; }
        public int DistrictID { get; set; }
        public District District { get; set; }
        public int WardID { get; set; }
        public Ward Ward { get; set; }
    }
}
