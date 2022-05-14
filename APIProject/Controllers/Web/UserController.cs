
/*--------------------------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 21.01.2022
 * Edit     : Chưa chỉnh Sửa
 * Content  : Lớp thông tin liên quan đến tài khoản của ADMin
 * -----------*/


using APIProject.Common.Models.Users;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService = null)
        {
            _userService = userService;
        }

        /// <summary>
        /// Thêm tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Nguyễn Thanh thủy - Dữ liệu mẫu:
        ///
        ///     {
        ///       "userName": "Nguyễn Thị Thanh Thủy",
        ///       "phone": "0968872539",
        ///       "email": "nguyenthithanhthuy@gmail.com",
        ///       "password": "123456aA",
        ///       "role": 1
        ///     }
        ///
        /// </remarks>
        [HttpPost("CreateUser")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_USER)]
        public async Task<JsonResultModel> CreateUser([FromBody] CreateUserModel model)
        {
            return await _userService.CreateUser(model);
        }
        /// <summary>
        /// Xóa tài khoản 
        /// </summary>
        /// <param name="ID">ID của tài khoản</param>
        /// <returns></returns>
        [HttpDelete("DeleteUser/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_USER)]
        public async Task<JsonResultModel> DeleteUser(int ID)
        {
            return await _userService.DeleteUser(ID);
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_USER)]
        public async Task<JsonResultModel> UpdateUser(UpdateUserModel model)
        {
            return await _userService.UpdateUser(model);
        }
        /// <summary>
        /// Chi tiết tài khoản 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetail/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_USER)]
        public async Task<JsonResultModel> GetUserDetail(int ID)
        {
            return await _userService.GetUserDetail(ID);
        }
        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="SearchKey">Tên hoặc số điện thoại</param>
        /// <param name="role">Quyền</param>
        /// <param name="status"> 0 :Ngừng hoạt động, 1 :Hoạt động</param>
        /// <param name="fromDate">dd/MM/yyyy</param>
        /// <param name="toDate">dd/MM/yyyy</param>
        /// <returns></returns>
        [HttpGet("GetListUser")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_USER)]
        public async Task<JsonResultModel> GetListUser(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null, int? role = null, int? status = null, string fromDate = null, string toDate = null )
        {
            return await _userService.GetListUser(page, limit, SearchKey, role, status, fromDate, toDate);

        }
    }
}
