
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 16/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : ADMin Controler cung 
 * ----------------------------------*/

using APIProject.Service.Models;
using APIProject.Middleware;
using APIProject.Service.Interface;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using APIProject.Service.Models.Authentication;
using APIProject.Domain.Models;
using APIProject.Service.Interfaces;
using APIProject.Common.Models.Password;
using APIProject.Common.Models.Users;

namespace APIProject.Controllers.Web
{

    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IConfiguration _Configuration;
        private readonly IUserService _userService;
        private readonly string secretKey;
        private readonly int timeout;
        public AuthenticationController( IConfiguration configuration, IUserService userService)
        { 
            _Configuration = configuration;
            _userService = userService;
            try
            {
                secretKey = _Configuration["AppSettings:Secret"];
                timeout = int.Parse(_Configuration["Time:timeout"]);
            }
            catch
            {
                secretKey = String.Empty;
                timeout = 5;
            }
        }

        /// <summary>
        /// Đăng nhập cho Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "phone"    : "0912270312",
        ///       "password" : "123456"
        ///     }
        ///
        /// </remarks>
        [HttpPost("Login")]
        public async Task<JsonResultModel> Login(LoginModel model)
        {
            return await _userService.Authenticate(model, secretKey, timeout);
        }
        /// <summary>
        /// Lấy thông tin của Admin 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetUserInfo")]
        public JsonResultModel GetUserInfo()
        {
            return JsonResponse.Success((User)HttpContext.Items["Payload"]);
        }
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<JsonResultModel> ChangePassword(ChangePasswordWebModel model)
        {
            var user = (User)HttpContext.Items["Payload"];
            return await _userService.ChangePassword(user.ID,model.OldPassword,model.NewPassword);
        }
    }
}
