using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class District
    {
        [Key]
        public int Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        public int ProvinceCode { get; set; }

    }
}
