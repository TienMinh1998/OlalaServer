using APIProject.Common.Models.StorageExport;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStorageExportService
    {
        Task<JsonResultModel> GetListStorageExport(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate);
        Task<JsonResultModel> GetStorageExportDetail(int ID);
        Task<JsonResultModel> ExportStorage(CreateStorageExportModel input);
    }
}
