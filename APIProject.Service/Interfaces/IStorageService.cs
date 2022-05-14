using APIProject.Service.Models;
using APIProject.Service.Models.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStorageService
    {
        Task<JsonResultModel> GetListStorage();
        Task<JsonResultModel> GetStorageDetail(int ID);
        Task<JsonResultModel> UpdateStorage(StorageModel input);
    }
}
