using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        Task<List<int>> GetListRolePermission(int RoleID);
    }
}
