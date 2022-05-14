using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Config;
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
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }
        /// <summary>
        /// Danh sách thiết lập tích điểm
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CONFIG)]
        [HttpGet("GetListCustomerType")]
        public async Task<JsonResultModel> GetListCustomerType()
        {
            return await _configService.GetListCustomerType();
        }
        /// <summary>
        /// Danh sách thiết lập liên hệ
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CONFIG)]
        [HttpGet("GetListContact")]
        public async Task<JsonResultModel> GetListContact()
        {
            return await _configService.GetListContact();
        }
        /// <summary>
        /// Cập nhật thiết lập tích điểm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CONFIG)]
        [HttpPost("UpdateCustomerType")]
        public async Task<JsonResultModel> UpdateCustomerType([FromBody]UpdateCustomerTypeModel input)
        {
            return await _configService.UpdateCustomerType(input);
        }
        /// <summary>
        /// Cập nhật thiết lập liên hệ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CONFIG)]
        [HttpPost("UpdateContact")]
        public async Task<JsonResultModel> UpdateContact([FromBody] UpdateContactModel input)
        {
            return await _configService.UpdateContact(input);
        }
    }
}
