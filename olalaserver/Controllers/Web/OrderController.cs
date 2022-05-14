
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 10/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : Cung cấp các API liên quan đến Customer 
 *            -Hoàn thành API lấy danh sách đơn hàng Web  
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
using APIProject.Service.Interfaces;
using APIProject.Service.Models.Order;

namespace APIProject.Controllers
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public IConfiguration _Configuration;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// lấy danh sách đơn hàng Web, Tìm kiếm theo các điều kiện
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="searchKey">Có thể tìm theo mã sản phẩm, Tên khách hàng, Số điện thoại</param>
        /// <param name="status">Trạng thái</param>
        /// <param name="page">Trang thứ</param>
        /// <param name="limit">Giới hạn bản ghi</param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ORDER)]
        [HttpGet("GetListOrder")]
        public async Task<JsonResultModel> GetListOrder(string startDate = null, string endDate = null, string searchKey = null, int? status = null, int page = 1, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _orderService.GetListOrder(page, limit, searchKey, status, startDate, endDate);
        }


        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrderDetail/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ORDER)]
        public async Task<JsonResultModel> GetOrderDetail(int ID)
        {
            return await _orderService.GetOrderDetail(ID);
        }
        /// <summary>
        /// Tạo phiếu xuất kho
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetExportStorageForm/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ORDER)]
        public async Task<JsonResultModel> GetExportStorageForm(int ID)
        {
            return await _orderService.GetExportStorageForm(ID);
        }
        /// <summary>
        /// Thay đổi trạng thái đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "ID": 1,
        ///        "Status": 1
        ///     }
        ///
        /// </remarks>
        [HttpPost("ChangeStatusOrder")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ORDER)]
        public async Task<JsonResultModel> ChangeStatusOrder([FromBody] ChangeStatusOrderModel input)
        {
            return await _orderService.ChangeStatusOrder(input);
        }
        /// <summary>
        /// Xử lý yêu cầu hủy đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "ID": 1,
        ///        "IsCancel": 1
        ///     }
        ///
        /// </remarks>
        [HttpPost("HandleRequestCancelOrder")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ORDER)]
        public async Task<JsonResultModel> HandleRequestCancelOrder([FromBody] HandleRequestCancelOrderModel input)
        {
            return await _orderService.HandleRequestCancelOrder(input.ID, input.IsCancel);
        }
        /// <summary>
        /// Báo giá vận chuyển đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "ID": 1,
        ///        "ShipFee": 15000
        ///     }
        ///
        /// </remarks>
        [HttpPost("ShipQuoteOrder")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_ORDER)]
        public async Task<JsonResultModel> ShipQuoteOrder([FromBody] ShipQuoteOrderModel input)
        {
            return await _orderService.ShipQuoteOrder(input);
        }

    }
}
