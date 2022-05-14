using APIProject.Common.Models.Role;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }
        /// <summary>
        /// Danh sách quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListPermission")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ROLE)]
        public async Task<JsonResultModel> GetListPermission()
        {
            var model = await _permissionService.GetListPermission();
            return model;
        }
        /// <summary>
        /// Danh sách phân quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListRole")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ROLE)]
        public async Task<JsonResultModel> GetListRole()
        {
            var model = await _roleService.GetListRole();
            return model;
        }
        /// <summary>
        /// Danh sách phân quyền không quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListRoleNoPermission")]
        public async Task<JsonResultModel> GetListRoleNoPermission()
        {
            var model = await _roleService.GetListRole();
            return model;
        }
        /// <summary>
        /// Chi tiết phân quyền
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetRoleDetail/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ROLE)]
        public async Task<JsonResultModel> GetRoleDetail(int ID)
        {
            var model = await _roleService.GetRoleDetail(ID);
            return model;
        }
        /// <summary>
        /// Tạo phân quyền
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "Kế toán",
        ///        "ListPermissionID":[2,3,4,5]
        ///     }
        ///
        /// </remarks> 
        [HttpPost("CreateRole")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ROLE)]
        public async Task<JsonResultModel> CreateRole([FromBody]CreateRoleModel input)
        {
            var model = await _roleService.CreateRole(input);
            return model;
        }
        /// <summary>
        /// Sửa phân quyền
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "Kế toán cấp cao",
        ///        "listPermissionID":[3,4,5,6]
        ///     }
        ///
        /// </remarks> 
        [HttpPost("UpdateRole")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ROLE)]
        public async Task<JsonResultModel> UpdateRole([FromBody] UpdateRoleModel input)
        {
            var model = await _roleService.UpdateRole(input);
            return model;
        }
        /// <summary>
        /// Xóa phân quyền
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRole/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ROLE)]
        public async Task<JsonResultModel> DeleteRole(int ID)
        {
            var model = await _roleService.DeleteRole(ID);
            return model;
        }
    }
}
