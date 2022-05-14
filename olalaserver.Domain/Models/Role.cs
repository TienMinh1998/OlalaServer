using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
   public class Role : BaseModel
    {
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
