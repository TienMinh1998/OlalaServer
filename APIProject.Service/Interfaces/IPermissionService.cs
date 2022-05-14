using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IPermissionService : IServices<Permission>
    {
        Task<JsonResultModel> GetListPermission();
        Task<List<int>> GetListRolePermission(int RoleID);
    }
}
