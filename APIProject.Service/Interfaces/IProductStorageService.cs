using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IProductStorageService : IServices<ProductStorage>
    {
        Task<int?> GetProductQuantity(int ProductID);
        Task<JsonResultModel> GetListProductStorage(int Page, int Limit, string SearchKey, int? StorageID);
        Task<JsonResultModel> GetListProductStorageByProduct(int Page, int Limit, string SearchKey);
        Task<JsonResultModel> GetProductStorageHistory(int Page, int Limit, int ProductStorageID);
        Task<JsonResultModel> GetProductStorageDetail(int ProductStorageID);
    }
}
