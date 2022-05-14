using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
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
    public class OverviewController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public OverviewController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        /// <summary>
        /// Tổng quan
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOverview")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_OVERVIEW)]
        public async Task<JsonResultModel> GetOverview()
        {
            return await _statisticService.GetOverview();
        }
    }
}
