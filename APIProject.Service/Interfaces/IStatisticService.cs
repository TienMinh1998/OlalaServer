using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStatisticService
    {
        Task<JsonResultModel> GetListSales(int page, int limit, string orderCode, string customerName, string startDate, string endDate);
        Task<JsonResultModel> GetTotalSale(string orderCode, string customerName, string startDate, string endDate);
        Task<JsonResultModel> GetOverview();

    }
}
