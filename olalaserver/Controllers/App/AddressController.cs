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
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _AddressService;

        public AddressController(IAddressService AddressService)
        {
            _AddressService = AddressService;
        }
        /// <summary>
        /// Danh sách tỉnh/thành phố
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListProvince")]
        public async Task<JsonResultModel> GetListProvince()
        {
            return await _AddressService.GetProvinces();
        }
        /// <summary>
        /// Danh sách quận huyện
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListDistrict")]
        public async Task<JsonResultModel> GetListDistrict(int ProvinceID)
        {
            return await _AddressService.GetDistricts(ProvinceID);
        }
        /// <summary>
        /// Danh sách phường xã
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListWard")]
        public async Task<JsonResultModel> GetListWard(int DistrictID)
        {
            return await _AddressService.GetWards(DistrictID);
        }

    }
}
