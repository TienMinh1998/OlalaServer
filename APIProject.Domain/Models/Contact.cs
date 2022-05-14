using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Contact
    {
        [Key]
        public int ID { get; set; } 
        public string Name { get; set; } 
        public string Value { get; set; } 
    }
}
