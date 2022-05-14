using APIProject.Common.Models.ProductStorage;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IProductStorageRepository : IRepository<ProductStorage>
    {
        Task<IPagedList<ProductStorageModel>> GetProductStorages(int Page, int Limit, string SearchKey, int? StorageID);
        Task<IPagedList<ProductStorageByProductModel>> GetProductStoragesByProduct(int Page, int Limit, string SearchKey);
        Task<ProductStorageDetailModel> GetProductStorageDetail(int ProductStorageID);
    }
}
