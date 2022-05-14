using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using APIProject.Service.Utils;
using APIProject.Common.Models.ReceiveAddress;
using System.Threading.Tasks;

namespace APIProject.Repository
{
    public class ReceiveAddressRepository : BaseRepository<ReceiveAddress>, IReceiveAddressRepository
    {
        public ReceiveAddressRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<ReceiveAddressModel>> GetReceiveAddresses(int CusID, string Search)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var data = DbContext.ReceiveAddresses
                    .Where(x => x.CustomerID.Equals(CusID) && x.IsActive.Equals(SystemParam.ACTIVE)
                    && (!String.IsNullOrEmpty(Search) ? (x.Name.Contains(Search) || x.Phone.Contains(Search)
                    || x.Province.Name.Contains(Search) || x.District.Name.Contains(Search) || x.Ward.Name.Contains(Search)
                    || x.Address.Contains(Search)) : true)
                ).Select(x => new ReceiveAddressModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    Phone = x.Phone,
                    Address = x.Address,
                    DistrictID = x.DistrictID,
                    ProvinceID = x.ProvinceID,
                    WardID = x.WardID,
                    District = x.District.Name,
                    Province = x.Province.Name,
                    Ward = x.Ward.Name,
                    IsDefault = x.IsDefault
                }).OrderByDescending(x => x.IsDefault).ThenByDescending(x => x.ID).ToList();
                    return data;
                });

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ReceiveAddressModel> GetReceiveAddressDefault(int CusID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var data = DbContext.ReceiveAddresses.Where(x => x.CustomerID.Equals(CusID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.IsDefault.Equals(SystemParam.RECEIVE_ADDRESS_DEFAULT)).Select(x => new ReceiveAddressModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Phone = x.Phone,
                        Address = x.Address,
                        DistrictID = x.DistrictID,
                        ProvinceID = x.ProvinceID,
                        WardID = x.WardID,
                        District = x.District.Name,
                        Province = x.Province.Name,
                        Ward = x.Ward.Name,
                        IsDefault = x.IsDefault
                    }).FirstOrDefault();
                    return data;
                });

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ReceiveAddressModel> GetReceiveAddressDetail(int ID)
        {
            try
            {
                return await Task.Run(() =>
                {

                    var data = DbContext.ReceiveAddresses.Where(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE)).Select(x => new ReceiveAddressModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Phone = x.Phone,
                        Address = x.Address,
                        DistrictID = x.DistrictID,
                        ProvinceID = x.ProvinceID,
                        WardID = x.WardID,
                        District = x.District.Name,
                        Province = x.Province.Name,
                        Ward = x.Ward.Name,
                        IsDefault = x.IsDefault
                    }).FirstOrDefault();
                    return data;
                });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
