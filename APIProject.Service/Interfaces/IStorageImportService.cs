using APIProject.Common.Models.StorageImport;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStorageImportService
    {
        Task<JsonResultModel> GetListStorageImport(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate);
        Task<JsonResultModel> GetStorageImportDetail(int ID);
        Task<JsonResultModel> ImportStorage(CreateStorageImportModel input);
    }
}
