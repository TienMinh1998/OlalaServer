
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 10/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : Cung cấp các API liên quan đến Customer 
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

namespace APIProject.Controllers
{

    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public IConfiguration _Configuration;
        public CustomerController(ICustomerService customerService, IConfiguration configuration)
        {
            _customerService = customerService;
            _Configuration = configuration;
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="customerType"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CUSTOMER)]
        [HttpGet("GetCustomers")]
        public async Task<JsonResultModel> GetListCustomer(string startDate = null, string endDate = null, string searchKey = null, int? customerType = null, int? status = null, int page = 1, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _customerService.GetCustomers(page, limit, customerType, status, searchKey, startDate, endDate);
        }

        /// <summary>
        /// Duyện quyền cho khách hảng
        /// </summary>
        /// <param name="IDs">Danh sách ID cần Duyệt</param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CUSTOMER)]
        [HttpPut("ConfirmRole")]
        public async Task<JsonResultModel> ConfirmRole(int[] IDs)
        {
            return await _customerService.ConfirmRole(IDs);
        }

        /// <summary>
        /// Thay đổi trạng thái hoạt động của Customer
        /// </summary>
        /// <param name="customerID">ID của khách hàng</param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CUSTOMER)]
        [HttpPut("ChangeStatus")]
        public async Task<JsonResultModel> ChangeStatus([FromBody] int customerID)
        {
            return await _customerService.ChangeStatus(customerID);
        }

    }
}
