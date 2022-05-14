
/*--------------------------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 10/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : Login, Authentication, ForgotPassword  
 * -------------------------------------------------*/


using APIProject.Service.Utils;
using APIProject.Service.Models;
using APIProject.Middleware;
using APIProject.Service.Interface;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using APIProject.Service.Models.Authentication;
using APIProject.Domain.Models;
using APIProject.Common.Models.Password;
using Microsoft.AspNetCore.Hosting;
using APIProject.Service.Interfaces;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private readonly ICustomerService _customerService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string secretKey;
        private readonly int timeout;
        public AuthenticationController(ICustomerService customerService, IConfiguration configuration, IUploadFileService uploadFileService, IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _Configuration = configuration;
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
            _uploadFileService = uploadFileService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Lấy thông tin khách hàng App
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetUserInfo")]
        public JsonResultModel GetUserInfo()
        {
            return JsonResponse.Success((Customer)HttpContext.Items["Payload"]);
        }
        /// <summary>
        /// Đăng nhập ứng dụng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "phone": "0912270312",
        ///       "password": "123456"
        ///     }
        ///     {
        ///       "phone": "0968872539",
        ///       "password": "123456"
        ///     }
        ///
        /// </remarks>
        [HttpPost("Login")]
        public async Task<JsonResultModel> Login(LoginModel model)
        {
            return await _customerService.Authenticate(model, secretKey, timeout);
        }
        /// <summary>
        /// Đăng xuất ứng dụng
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        [Authorize]
        public async Task<JsonResultModel> Logout()
        {
            try
            {
                var cus = (Customer)HttpContext.Items["Payload"];
                cus.Token = String.Empty;
                await _customerService.UpdateAsync(cus);
                return JsonResponse.Success();
            }
            catch (Exception)
            {
                return JsonResponse.ServerError();
            }

        }
        /// <summary>
        /// Đăng kí người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///       "phone": "0968872539",
        ///       "name": "Nguyễn Viết Minh Tiến",
        ///       "password": "123456",
        ///       "email": "windsoft@gmail.com",
        ///       "customerTypeID": 1,
        ///       "codeTax": ""
        ///     }
        ///
        /// </remarks>
        [HttpPost("Register")]
        public async Task<JsonResultModel> Register([FromForm] RegisterModel model)
        {
            model.Avatar = _uploadFileService.UploadImage(SystemParam.FILE_NAME, HttpContext, _webHostEnvironment);
            return await _customerService.Register(model, secretKey, timeout);
        }

      /// <summary>
      /// Quên mật khẩu
      /// </summary>
      /// <param name="input"></param>
      /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public async Task<JsonResultModel> ForgotPassword([FromBody] ForgotPasswordModel input)
        {
            return await _customerService.ForgotPassword(input.Phone);
        }
        /// <summary>
        /// Xác nhận mã OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ConfirmOTP")]
        public async Task<JsonResultModel> ConfirmOTP([FromBody] ConfirmOTPModel model)
        {
            return await _customerService.ConfirmOTP(model);
        }
        /// <summary>
        /// Thay đổi mật khẩu khi đã xác nhận OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ChangePasswordOTP")]
        public async Task<JsonResultModel> ChangePasswordOTP([FromBody] ChangePasswordOTPModel model)
        {
            return await _customerService.ChangePasswordOTP(model);
        }
        /// <summary>
        /// Thay đổi mật khẩu 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        public async Task<JsonResultModel> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _customerService.ChangePassword(cus, model.Password);
        }
    }
}
