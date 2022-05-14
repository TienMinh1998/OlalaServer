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
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }


        /// <summary>
        /// Danh sách doanh số
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="orderCode"></param>
        /// <param name="customerName"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        [HttpGet("GetListSales")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STATISTIC)]
        public async Task<JsonResultModel> GetListSales(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT, string orderCode = null, string customerName = null, string fromdate = null, string todate = null)
        {
            return await _statisticService.GetListSales(page, limit, orderCode, customerName, fromdate, todate);
        }

        /// <summary>
        /// Tổng doanh thu
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="customerName"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        [HttpGet("GetTotalSale")]
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_STATISTIC)]
        public async Task<JsonResultModel> GetTotalSale(string orderCode = null, string customerName = null, string fromdate = null, string todate = null)
        {
            return await _statisticService.GetTotalSale(orderCode, customerName, fromdate, todate);
        }

        

    }
}
