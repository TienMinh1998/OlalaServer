using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Permission
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
