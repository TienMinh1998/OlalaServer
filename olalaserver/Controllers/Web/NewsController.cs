using APIProject.Common.Models.News;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHub _sentry;
        public NewsController(INewsService newsService, IWebHostEnvironment webHostEnvironment, IUploadFileService uploadFileService, IHub sentry)
        {
            _newsService = newsService;
            _webHostEnvironment = webHostEnvironment;
            _uploadFileService = uploadFileService;
            _sentry = sentry;

        }
        /// <summary>
        /// Thêm tin tức
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Title":"Tiêu đề",
        ///        "Content": "Nội dung",
        ///        "Type": 1,
        ///        "TypeNews": 1,
        ///        "Status": "CH001",
        ///        "SentNotification": true
        ///     }
        ///
        /// </remarks>
        [HttpPost("AddNews")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_NEWS)]
        public async Task<JsonResultModel> AddNews([FromBody] NewAddInputModel input)
        {
            return await _newsService.AddNews(input);
        }
        /// <summary>
        /// Thêm tin tức
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "ID":1,
        ///        "Title":"Tiêu đề",
        ///        "Content": "Nội dung",
        ///        "Type": 1,
        ///        "TypeNews": 1,
        ///        "Status": "CH001",
        ///        "SentNotification": true
        ///     }
        ///
        /// </remarks>
        [HttpPut("UpdateNews")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_NEWS)]
        public async Task<JsonResultModel> UpdateNews([FromBody] UpdateNewsModel input)
        {
            return await _newsService.UpdateNews(input);
        }
        /// <summary>
        /// Thay đổi trạng thái hoạt động
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPut("ChangeStatusNews")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_NEWS)]
        public async Task<JsonResultModel> ChangeStatusNews(int ID)
        {
            return await _newsService.ChangeStatusNews(ID);
        }
        /// <summary>
        /// Danh sách tin tức 
        /// </summary>
        /// <param name="fromDate">dd/MM/yyyy</param>
        /// <param name="toDate">dd/MM/yyyy</param>
        /// <param name="TitleBanner"></param>
        /// <param name="Type">Trạng thái đăng bài (0 : Lưu nháp, 1: Đăng bài)</param>
        /// <param name="TypeNews">Loại tin tức (0 : Banner, 1: Trở thành đối tác)</param>
        /// <param name="status">Trạng tái của tin ( 0 : Tin không hoạt động, 1 : Tin hoạt động)</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetListNews")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_NEWS)]
        public async Task<JsonResultModel> GetListNews(string fromDate, string toDate, string TitleBanner = null, int? Type = null, int? TypeNews = null, int? status = null, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _newsService.GetListNews(page, limit, TitleBanner, Type, TypeNews, status, fromDate, toDate);
        }
        /// <summary>
        /// Chi tiết tin tức
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetNewsDetail/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_NEWS)]
        public async Task<JsonResultModel> GetNewsDetail(int ID)
        {
            return await _newsService.GetNewsDetail(ID);
        }
        /// <summary>
        /// Xóa tin tức Web
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("DeleteNews/{ID}")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_NEWS)]
        public async Task<JsonResultModel> DeleteNews(int ID)
        {
            return await _newsService.DeleteNews(ID);
        }

    }
}
