

using APIProject.Service.Models;
using APIProject.Middleware;
using APIProject.Service.Interface;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using APIProject.Domain.Models;
using APIProject.Service.Interfaces;
using static APIProject.Service.Utils.SystemParam;
using System.Threading.Tasks;

namespace APIProject.Controllers
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly ICustomerService _customerService;
        private readonly IHomeService _homeService;
        private readonly IConfigService _configService;

        public HomeController(ICustomerService customerService, IHomeService homeService, IConfigService configService)
        {
            _customerService = customerService;
            _homeService = homeService;
            _configService = configService;
        }


        [HttpGet("GetHome")]
        public async Task<JsonResultModel> GetHome()
        {

            var cus = (Customer)HttpContext.Items["Payload"];
            var customerType = _customerService.GetCustomerType(cus);
            return await _homeService.GetHome(customerType);

        }
        /// <summary>
        /// Danh sách sản phẩm màn Home
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        [HttpGet("GetHomeProduct")]
        public async Task<JsonResultModel> GetHomeProduct(int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT)
        {

            var cus = (Customer)HttpContext.Items["Payload"];
            var customerType = _customerService.GetCustomerType(cus);
            return await _homeService.GetHomeProduct(Page, Limit, customerType);

        }
        [HttpGet("GetListContact")]
        public async Task<JsonResultModel> GetListContact()
        {
            return await _configService.GetListContact();
        }
    }
}
