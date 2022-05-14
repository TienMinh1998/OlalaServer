using APIProject.Common.Models.StorageImport;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IStorageImportRepository : IRepository<StorageImport>
    {
        Task<IPagedList<StorageImportModel>> GetStorageImports(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate);
        Task<StorageImportDetailModel> GetStorageImportDetail(int ID);
    }
}
