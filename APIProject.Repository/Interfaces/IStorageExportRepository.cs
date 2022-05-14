using APIProject.Common.Models.StorageExport;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IStorageExportRepository : IRepository<StorageExport>
    {
        Task<IPagedList<StorageExportModel>> GetStorageExports(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate);
        Task<StorageExportDetailModel> GetStorageExportDetail(int ID);
    }
}
