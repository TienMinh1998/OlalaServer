using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIProject.Common.Models.Order;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interface;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Order;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public OrderController(IOrderService orderService, IUploadFileService uploadFileService, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            _uploadFileService = uploadFileService;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "cartID": [1,2,3],
        ///        "receiveAddressID": 1,
        ///        "Note": "Giao nhanh giúp mình"
        ///     }
        ///
        /// </remarks>
        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<JsonResultModel> CreateOrder([FromBody] CreateOrderModel input)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _orderService.CreateOrder(input, cus.ID);
        }
        /// <summary>
        /// Danh sách đơn hàng
        /// </summary>
        /// <param name="Status">0:Báo giá vận chuyển , 1:Chờ xác nhận , 2:Đang giao , 3:Đã giao, 4:Hoàn thành , -1:Hủy , -2:Khiếu nại,-3:Trả hàng</param>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        [HttpGet("GetListOrder")]
        [Authorize]
        public async Task<JsonResultModel> GetListOrder(int Status, int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _orderService.GetListOrder(Page, Limit, Status, cus.ID);
        }
        /// <summary>
        /// Chi tiết đơn hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetOrderDetail/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> GetOrderDetail(int ID)
        {
            return await _orderService.GetOrderDetail(ID);
        }

        /// <summary>
        /// Yêu cầu hủy đơn hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("RequestCancelOrder/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> RequestCancelOrder(int ID)
        {
            return await _orderService.RequestCancelOrder(ID);
        }

        /// <summary>
        /// Thanh toán order 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("PaymentOrder")]
        public async Task<JsonResultModel> PaymentOrder([FromBody] PaymentOrderModel model)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _orderService.PaymentOrder(cus, model.OrderID, model.paymentType, model.UserPoint);
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("CancelOrder/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> CancelOrder(int ID)
        {
            return await _orderService.CancelOrder(ID);
        }
        /// <summary>
        /// Hoàn thành đơn hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("CompleteOrder/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> CompleteOrder(int ID)
        {
            return await _orderService.CompleteOrder(ID);
        }
        /// <summary>
        /// Khiếu nại đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "ID": 1,
        ///        "ListImageUrl": ["image1.jpg","image2.jpg"],
        ///        "NoteComplain": "Giao hàng quá chậm"
        ///     }
        ///
        /// </remarks>
        [HttpPost("ComplainOrder")]
        [Authorize]
        public async Task<JsonResultModel> ComplainOrder([FromForm] ComplainOrderModel input)
        {
            input.ListImageUrl = _uploadFileService.UploadImages(SystemParam.FILE_NAME, HttpContext, _webHostEnvironment);
            return await _orderService.ComplainOrder(input);

        }

    }
}