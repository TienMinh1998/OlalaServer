using APIProject.Common.Models.News;
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

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// lấy thông tin chi tiết của tin
        /// </summary>
        /// <param name="ID">ID của tin tức </param>
        /// <returns>Trả ra các thông tin của tin tức</returns>
        [HttpGet("GetNewsDetail/{ID}")]
        public async Task<JsonResultModel> GetNewsDetail(int ID)
        {
            return await _newsService.GetNewsDetail(ID);
        }
        /// <summary>
        /// Bài viết trở thành đối tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNewsPartner")]
        public async Task<JsonResultModel> GetNewsPartner()
        {
            return await _newsService.GetNewsPartner();
        }
    }
}
