using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IHomeService
    {
        Task<JsonResultModel> GetHome(int? CusType);
        Task<JsonResultModel> GetHomeProduct(int Page,int Limit,int? CusType);
    }
}
