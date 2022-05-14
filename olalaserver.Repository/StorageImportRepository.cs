using APIProject.Common.Models.StorageImport;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Utils;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class StorageImportRepository : BaseRepository<StorageImport>, IStorageImportRepository
    {
        public StorageImportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IPagedList<StorageImportModel>> GetStorageImports(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fromDate = Util.ConvertFromDate(FromDate);
                    var toDate = Util.ConvertToDate(ToDate);
                    var model = (from si in DbContext.StorageImports
                                 where si.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(SearchKey) ? si.Code.Contains(SearchKey) : true)
                                  && (StorageID.HasValue ? si.StorageID.Equals(StorageID) : true)
                                  && (fromDate.HasValue ? si.ImportDate >= fromDate : true)
                                  && (toDate.HasValue ? si.ImportDate <= toDate : true)
                                 orderby si.CreatedDate descending
                                 select new StorageImportModel
                                 {
                                     ID = si.ID,
                                     ImportDate = si.ImportDate,
                                     Storage = si.Storage.Name,
                                     TotalPrice = si.TotalPrice,
                                     Code = si.Code,
                                     TotalWeight = si.TotalWeight
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<StorageImportDetailModel> GetStorageImportDetail(int ID)
        {
            try
            {

                var model = await (from si in DbContext.StorageImports
                             where si.IsActive.Equals(SystemParam.ACTIVE) && si.ID.Equals(ID)
                             select new StorageImportDetailModel
                             {
                                 ID = si.ID,
                                 ImportDate = si.ImportDate,
                                 Storage = si.Storage.Name,
                                 TotalPrice = si.TotalPrice,
                                 TotalWeight = si.TotalWeight,
                                 StorageImportProducts = (from sid in DbContext.StorageImportDetails
                                                          join ps in DbContext.ProductStorages on sid.ProductStorageID equals ps.ID
                                                          join p in DbContext.Products on ps.ProductID equals p.ID
                                                          where sid.StorageImportID.Equals(si.ID) 
                                                          select new StorageImportProductModel
                                                          {
                                                              ID = sid.ID,
                                                              ExpiredDate = ps.ExpiredDate,
                                                              ManufactureDate = ps.ManufactureDate,
                                                              Name = p.Name,
                                                              Supplier = ps.Supplier,
                                                              Quantity = sid.Quantity,
                                                              Unit = p.Unit,
                                                              MinQuantityStorage = p.MinQuantityStorage,
                                                              Code = p.Code,
                                                              LotNo = ps.LotNo,
                                                              Size = p.Size,
                                                              Origin = p.Origin,
                                                              TotalPrice = sid.TotalPrice,
                                                              TotalWeight = sid.TotalWeight,
                                                              NetWeight = p.NetWeight,
                                                              Price = sid.Price,
                                                              Note = sid.Note
                                                          }).ToList()
                             }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
