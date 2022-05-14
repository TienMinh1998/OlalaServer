using APIProject.Common.Models.ReceiveAddress;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class ReceiveAddressController : ControllerBase
    {
        private readonly IReceiveAddressService _ReceiveAddressService;

        public ReceiveAddressController(IReceiveAddressService ReceiveAddressService)
        {
            _ReceiveAddressService = ReceiveAddressService;
        }
        /// <summary>
        /// Danh sách địa chỉ nhận hàng
        /// </summary>
        /// <param name="SearchKey">Tìm kiếm SĐT + Tên + Địa chỉ </param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetListReceiveAddress")]
        public async Task<JsonResultModel> GetListReceiveAddress(string SearchKey = null)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _ReceiveAddressService.GetReceiveAddresses(cus.ID, SearchKey);
        }
        /// <summary>
        /// Chi tiết địa chỉ nhận hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetReceiveAddressDetail/{ID}")]
        public async Task<JsonResultModel> GetReceiveAddressDetail(int ID)
        {
            return await _ReceiveAddressService.GetReceiveAddressDetail(ID);
        }
        /// <summary>
        /// Lấy địa chỉ nhận hàng
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetReceiveAddressDefault")]
        public async Task<JsonResultModel> GetReceiveAddressDefault()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _ReceiveAddressService.GetReceiveAddressDefault(cus.ID);
        }
        /// <summary>
        /// Thêm địa chỉ nhận hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Name": "Nguyễn Tú Đội",
        ///        "Phone": "0737232190",
        ///        "ProvinceID": 1,
        ///        "DistrictID": 1,
        ///        "WardID": 1,
        ///        "Address": "G2 Fivestar Kim Giang",
        ///        "IsDefault": 1
        ///     }
        ///
        /// </remarks>
        [Authorize]
        [HttpPost("CreateReceiveAddress")]
        public async Task<JsonResultModel> CreateReceiveAddress([FromBody] AddReceiveAddressModel input)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _ReceiveAddressService.CreateReceiveAddress(input,cus.ID);
        }
        /// <summary>
        /// Sửa địa chỉ nhận hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "ID": 1,
        ///        "Name": "Nguyễn Tú Đội",
        ///        "Phone": "0737232190",
        ///        "ProvinceID": 1,
        ///        "DistrictID": 1,
        ///        "WardID": 1,
        ///        "Address": "G2 Fivestar Kim Giang",
        ///        "IsDefault": 1
        ///     }
        ///
        /// </remarks>
        [Authorize]
        [HttpPut("UpdateReceiveAddress")]
        public async Task<JsonResultModel> UpdateReceiveAddress([FromBody] UpdateReceiveAddressModel input)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _ReceiveAddressService.UpdateReceiveAddress(input, cus.ID);
        }

        /// <summary>
        /// Xóa địa chỉ nhận hàng
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeleteReceiveAddress/{ID}")]
        public async Task<JsonResultModel> DeleteReceiveAddress(int ID)
        {
            return await _ReceiveAddressService.DeleteReceiveAddress(ID);
        }

    }
}
