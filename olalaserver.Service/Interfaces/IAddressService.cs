using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IAddressService
    {
        Task<JsonResultModel> GetProvinces();
        Task<JsonResultModel> GetDistricts(int ProvinceID);
        Task<JsonResultModel> GetWards(int DistrictID);
    }
}
