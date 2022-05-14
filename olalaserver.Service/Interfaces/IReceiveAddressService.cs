using APIProject.Common.Models.ReceiveAddress;
using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IReceiveAddressService : IServices<ReceiveAddress>
    {
        Task<JsonResultModel> GetReceiveAddresses(int CusID, string Search);
        Task<JsonResultModel> CreateReceiveAddress(AddReceiveAddressModel input, int CusID);
        Task<JsonResultModel> GetReceiveAddressDefault(int CusID);
        Task<JsonResultModel> GetReceiveAddressDetail(int ID);
        Task<JsonResultModel> UpdateReceiveAddress(UpdateReceiveAddressModel input, int CusID);
        Task<JsonResultModel> DeleteReceiveAddress(int ID);
    }
}
