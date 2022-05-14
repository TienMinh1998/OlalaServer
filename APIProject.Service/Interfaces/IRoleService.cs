using APIProject.Common.Models.Role;
using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IRoleService : IServices<Role>
    {
        Task<JsonResultModel> GetListRole();
        Task<JsonResultModel> GetRoleDetail(int ID);
        Task<JsonResultModel> CreateRole(CreateRoleModel input);
        Task<JsonResultModel> UpdateRole(UpdateRoleModel input);
        Task<JsonResultModel> DeleteRole(int ID);
    }
}
