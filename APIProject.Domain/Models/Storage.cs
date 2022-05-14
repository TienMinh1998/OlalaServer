using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Storage : BaseModel
    {
        [StringLength(100)]
        public string Name { get; set; }
        public string Address { get; set; }
        public int Status { get; set; } = SystemParam.ACTIVE;
        
    }
}
