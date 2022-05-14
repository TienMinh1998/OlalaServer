
using APIProject.Common.Models.ReceiveAddress;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IReceiveAddressRepository : IRepository<ReceiveAddress>
    {
        Task<List<ReceiveAddressModel>> GetReceiveAddresses(int CusID, string Search);
        Task<ReceiveAddressModel> GetReceiveAddressDefault(int CusID);
        Task<ReceiveAddressModel> GetReceiveAddressDetail(int ID);
    }

}
