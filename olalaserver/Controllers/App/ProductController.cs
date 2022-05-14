using APIProject.Service.Models;
using APIProject.Service.Utils;
using APIProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIProject.Domain.Models;
using APIProject.Service.Interface;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public ProductController(IProductService productService, ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }
        /// <summary>
        /// Lấy danh sách sản phẩm
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SearchKey"></param>
        /// <param name="Type">1:sản phẩm khuyến mãi , 2: sản phẩm hot , 3: sản phẩm bán chạy</param>
        /// <param name="SortPriceType">1:giá tăng dần , 2:giá giảm dần</param>
        /// <returns></returns>
        [HttpGet("GetListProduct")]
        public async Task<JsonResultModel> GetListProduct(int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null, int? Type = null, int? SortPriceType = null,int? CategoryID = null)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            var customerType = _customerService.GetCustomerType(cus);
            return await _productService.GetProducts(Page, Limit, SearchKey, customerType, Type, SortPriceType, CategoryID);
        }
        /// <summary>
        /// Lấy chi tiết sản phẩm
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetProductDetail/{ID}")]
        public async Task<JsonResultModel> GetProductDetail(int ID)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            var customerType = _customerService.GetCustomerType(cus);
            return await _productService.GetProductDetail(ID,customerType);
        }
    }
}
