
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 10/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : Cung cấp các API liên quan đến Customer 
 * ----------------------------------*/
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interface;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Customer;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public IConfiguration _Configuration;
        private readonly IUploadFileService _uploadFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CustomerController(ICustomerService customerService, IConfiguration configuration, IUploadFileService uploadFileService, IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _Configuration = configuration;
            _uploadFileService = uploadFileService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <returns></returns>
        [HttpPost("ChangeAvatar")]
        [Authorize]
        public async Task<JsonResultModel> ChangeAvatar()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            var avatar = _uploadFileService.UploadImage(SystemParam.FILE_NAME, HttpContext, _webHostEnvironment);
            return await _customerService.ChangeAvatar(cus, avatar);
        }
        /// <summary>
        /// Cập nhật thông tin cá nhân
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Name": "Tran Viet Anh Phuong",
        ///        "Email": "windsoftpro01@gmail.com",
        ///        "DOB": "06/11/1991",
        ///        "Gender": 1
        ///     }
        ///
        /// </remarks>
        [HttpPost("UpdateUserInfo")]
        [Authorize]
        public async Task<JsonResultModel> UpdateUserInfo([FromBody] ChangeCustomerInfoModel input)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _customerService.UpdateUserInfo(cus, input);
        }
        /// <summary>
        /// Lịch sử tích điểm
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="Type">null:Tất cả , 1:Đã tích , 2:Đã dùng</param>
        /// <param name="FromDate">dd/MM/yyyy</param>
        /// <param name="ToDate">dd/MM/yyyy</param>
        /// <returns></returns>
        [HttpGet("GetPointHistory")]
        [Authorize]
        public async Task<JsonResultModel> GetPointHistory(int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT, int Type = SystemParam.TYPE_MPH_ALL, string FromDate = null, string ToDate = null)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _customerService.GetMemberPointHistory(Page, Limit, Type, cus.ID, FromDate, ToDate);
        }
    }


}
