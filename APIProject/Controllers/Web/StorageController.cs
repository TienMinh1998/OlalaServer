using APIProject.Common.Models.StorageExport;
using APIProject.Common.Models.StorageImport;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Storage;
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
    public class StorageController : ControllerBase
    {
        IStorageService _storageService;
        IStorageImportService _storageImportService;
        IStorageExportService _storageExportService;
        IProductStorageService _productStorageService;

        public StorageController(IStorageService storageService, IStorageImportService storageImportService, IProductStorageService productStorageService, IStorageExportService storageExportService)
        {
            _storageService = storageService;
            _storageImportService = storageImportService;
            _productStorageService = productStorageService;
            _storageExportService = storageExportService;
        }

        /// <summary>
        /// Danh sách kho
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpGet("GetListStorage")]
        public async Task<JsonResultModel> GetListStorage()
        {
            return await _storageService.GetListStorage();
        }
        /// <summary>
        /// Chi tiết kho
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpGet("GetStorageDetail/{ID}")]
        public async Task<JsonResultModel> GetStorageDetail(int ID)
        {
            return await _storageService.GetStorageDetail(ID);
        }
        /// <summary>
        /// Cập nhật kho
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpPost("UpdateStorage")]
        public async Task<JsonResultModel> UpdateStorage([FromBody] StorageModel input)
        {
            return await _storageService.UpdateStorage(input);
        }
        /// <summary>
        /// Danh sách nhập kho
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SearchKey">Mã nhập kho</param>
        /// <param name="StorageID"></param>
        /// <param name="FromDate">dd/MM/yyyy</param>
        /// <param name="ToDate">dd/MM/yyyy</param>
        /// <returns></returns>
        [HttpGet("GetListStorageImport")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        public async Task<JsonResultModel> GetListStorageImport(int Page = 1, int Limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null, int? StorageID = null, string FromDate = null, string ToDate = null)
        {
            return await _storageImportService.GetListStorageImport(Page, Limit, SearchKey, StorageID, FromDate, ToDate);

        }
        /// <summary>
        /// Chi tiết nhập kho
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpGet("GetStorageImportDetail/{ID}")]
        public async Task<JsonResultModel> GetStorageImportDetail(int ID)
        {
            return await _storageImportService.GetStorageImportDetail(ID);
        }
        /// <summary>
        /// Nhập kho
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "storageID": 1,
        ///        "importDate": 15/01/2022,
        ///        "storageImportProducts":[
        ///          {
        ///            "productID": 1,
        ///            "supplier": "Indochina",
        ///            "lotNo": "PH0001",
        ///            "quantity": 15,
        ///            "price": 200000,
        ///            "ManufactureDate": 08/10/2021,
        ///            "ExpiredDate": 08/12/2024
        ///          }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpPost("ImportStorage")]
        public async Task<JsonResultModel> ImportStorage(CreateStorageImportModel input)
        {
            return await _storageImportService.ImportStorage(input);
        }
        /// <summary>
        /// Danh sách xuất kho
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SearchKey">Mã xuất kho</param>
        /// <param name="StorageID"></param>
        /// <param name="FromDate">dd/MM/yyyy</param>
        /// <param name="ToDate">dd/MM/yyyy</param>
        /// <returns></returns>
        [HttpGet("GetListStorageExport")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        public async Task<JsonResultModel> GetListStorageExport(int Page = 1, int Limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null, int? StorageID = null, string FromDate = null, string ToDate = null)
        {
            return await _storageExportService.GetListStorageExport(Page, Limit, SearchKey, StorageID, FromDate, ToDate);

        }
        /// <summary>
        /// Chi tiết xuất kho
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpGet("GetStorageExportDetail/{ID}")]
        public async Task<JsonResultModel> GetStorageExportDetail(int ID)
        {
            return await _storageExportService.GetStorageExportDetail(ID);
        }
        /// <summary>
        /// Xuất kho
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "storageID": 1,
        ///        "exportDate": 15/01/2022,
        ///        "condition": "Điều kiện khác",
        ///        "reason": "Lý do xuất kho",
        ///        "note": "Ghi chú",
        ///        "customer": "Khách hàng",
        ///        "receiverName": "Người nhận",
        ///        "provinceID": 1,
        ///        "numberCar": "ABCD",
        ///        "storageExportProducts":[
        ///          {
        ///            "productStorageID": 1,
        ///            "storageID": 1,
        ///            "quantity": 15,
        ///            "price": 200000
        ///          }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        [HttpPost("ExportStorage")]
        public async Task<JsonResultModel> ExportStorage(CreateStorageExportModel input)
        {
            return await _storageExportService.ExportStorage(input);
        }
        /// <summary>
        /// Danh sách tồn kho theo xuất/nhập kho
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SearchKey">Mã,tên sản phẩm</param>
        /// <param name="StorageID"></param>
        /// <returns></returns>
        [HttpGet("GetListProductStorage")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        public async Task<JsonResultModel> GetListProductStorage(int Page = 1, int Limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null, int? StorageID = null)
        {
            return await _productStorageService.GetListProductStorage(Page, Limit, SearchKey, StorageID);

        }
        /// <summary>
        /// Danh sách tồn kho theo sản phẩm
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="SearchKey">Mã,tên sản phẩm</param>
        /// <returns></returns>
        [HttpGet("GetListProductStorageByProduct")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        public async Task<JsonResultModel> GetListProductStorageByProduct(int Page = 1, int Limit = SystemParam.LIMIT_DEFAULT, string SearchKey = null)
        {
            return await _productStorageService.GetListProductStorageByProduct(Page, Limit, SearchKey);

        }
        /// <summary>
        /// Chi tiết tồn kho
        /// </summary>
        /// <param name="ProductStorageID"></param>
        /// <returns></returns>
        [HttpGet("GetProductStorageDetail")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        public async Task<JsonResultModel> GetProductStorageDetail(int ProductStorageID)
        {
            return await _productStorageService.GetProductStorageDetail(ProductStorageID);

        }
        /// <summary>
        /// Lịch sử tồn kho
        /// </summary>
        /// <param name="ProductStorageID"></param>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        [HttpGet("GetProductStorageHistory")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STORAGE)]
        public async Task<JsonResultModel> GetProductStorageHistory(int ProductStorageID, int Page = 1, int Limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _productStorageService.GetProductStorageHistory(Page, Limit, ProductStorageID);

        }
    }
}
