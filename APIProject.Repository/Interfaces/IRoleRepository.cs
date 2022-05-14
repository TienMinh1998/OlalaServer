using APIProject.Common.Models.Role;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<List<RoleModel>> GetListRole();
        Task<RoleModel> GetRoleDetail(int ID);
    }
}
