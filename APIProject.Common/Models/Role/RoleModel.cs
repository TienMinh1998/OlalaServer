using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Role
{
    public class RoleModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<PermissionModel> ListPermission { get; set; }
    }
    public class PermissionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class CreateRoleModel
    {
        public string Name { get; set; }
        public List<int> ListPermissionID { get; set; }
    }
    public class UpdateRoleModel : CreateRoleModel
    {
        public int ID { get; set; }
    }
}
