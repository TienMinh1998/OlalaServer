using APIProject.Common.Models.ProductStorage;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IProductStorageHistoryRepository : IRepository<ProductStorageHistory>
    {
        Task<IPagedList<ProductStorageHistoryModel>> GetProductStorageHistory(int Page, int Limit, int ProductStorageID);
    }
}
