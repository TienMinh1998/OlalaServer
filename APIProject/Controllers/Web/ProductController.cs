using APIProject.Service.Models;
using APIProject.Service.Utils;
using APIProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using APIProject.Middleware;
using APIProject.Common.Models.Product;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SearchKey"></param>
        /// <param name="Status">1:đang hoạt đông , 0: ngừng hoạt động</param>
        /// <param name="FromDate">dd/MM/yyyy</param>
        /// <param name="ToDate">dd/MM/yyyy</param>
        /// <returns></returns>
        [HttpGet("GetListProduct")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_PRODUCT)]
        public async Task<JsonResultModel> GetListProduct(int Page = 1, int Limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null, int? Status = null, string FromDate = null, string ToDate = null)
        {
            return await _productService.GetProducts(Page, Limit, SearchKey, Status, FromDate, ToDate);
        }

        /// <summary>
        /// Tạo sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "code": "CH001",
        ///        "name": "Cá hồi ướp lạnh",
        ///        "unit": "Khay",
        ///        "size": "400-600 gram/con hoặc 500 gram+/con",
        ///        "usage": "gia nhiệt trước khi dùng, không tái đông sau khi rã đông",
        ///        "netWeight": 2.5,
        ///        "minQuantityStorage": 1,
        ///        "origin": "Châu Âu",
        ///        "storageTemperature": "Nhiệt độ bảo quản 20 độ C",
        ///        "ingredient": "Cá",
        ///        "type": 1,
        ///        "categoryID": 7,
        ///        "isNotify": 0,
        ///        "listImage": ["image123.jpg","image321.png"],
        ///        "description":"mô tả",
        ///        "listProductItem":[
        ///          {
        ///            "customerType": 1,
        ///            "price": 200000,
        ///            "originalPrice": 160000
        ///          }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost("CreateProduct")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_PRODUCT)]
        public async Task<JsonResultModel> CreateProduct([FromBody] CreateProductModel input)
        {
            return await _productService.CreateProduct(input);
        }
        /// <summary>
        /// Sửa sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id":20,
        ///        "code": "CH001",
        ///        "name": "Cá hồi ướp lạnh",
        ///        "unit": "Khay",
        ///        "size": "400-600 gram/con hoặc 500 gram+/con",
        ///        "usage": "gia nhiệt trước khi dùng, không tái đông sau khi rã đông",
        ///        "netWeight": 2.5,
        ///        "minQuantityStorage": 1,
        ///        "origin": "Châu Âu",
        ///        "storageTemperature": "Nhiệt độ bảo quản 20 độ C",
        ///        "ingredient": "Cá",
        ///        "type": 1,
        ///        "categoryID": 7,
        ///        "listImage": ["image123.jpg","image321.png"],
        ///        "description":"mô tả",
        ///        "listProductItemCreate":[
        ///          {
        ///            "id":0,
        ///            "customerType": 4,
        ///            "price": 350000,
        ///            "originalPrice": 160000
        ///          },
        ///          {
        ///            "id":0,
        ///            "customerType": 2,
        ///            "price": 280000,
        ///            "originalPrice": 360000
        ///          },
        ///          {
        ///            "id":0,
        ///            "customerType": 3,
        ///            "price": 200000,
        ///            "originalPrice": 460000
        ///          }
        ///        ],
        ///        "listProductItemUpdate":[
        ///          {
        ///            "id":0,
        ///            "customerType": 1,
        ///            "price": 200000,
        ///            "originalPrice": 160000
        ///          }
        ///        ],
        ///        "listProductItemDelete":[
        ///          {
        ///            "id":0,
        ///            "customerType": 1,
        ///            "price": 200000,
        ///            "originalPrice": 160000
        ///          }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPut("UpdateProduct")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_PRODUCT)]
        public async Task<JsonResultModel> UpdateProduct([FromBody] UpdateProductModel input)
        {
            return await _productService.UpdateProduct(input);
        }
        /// <summary>
        /// Thay đổi trạng thái sản phẩm
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPut("UpdateProductStatus/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_PRODUCT)]
        public async Task<JsonResultModel> UpdateProductStatus(int ID)
        {
            return await _productService.UpdateProductStatus(ID);
        }
        /// <summary>
        /// Chi tiết sản phẩm 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetProductDetail/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_PRODUCT)]
        public async Task<JsonResultModel> GetProductDetail(int ID)
        {
            return await _productService.GetProductDetail(ID);
        }
        /// <summary>
        /// Xóa sản phẩm 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_PRODUCT)]
        public async Task<JsonResultModel> DeleteProduct(int ID)
        {
            return await _productService.DeleteProduct(ID);
        }


    }
}
