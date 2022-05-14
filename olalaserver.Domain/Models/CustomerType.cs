
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIProject.Domain.Models
{
    public class CustomerType
    {
        [Key]
        public int ID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public long BonusPointPerKg { get; set; }
    }
}
