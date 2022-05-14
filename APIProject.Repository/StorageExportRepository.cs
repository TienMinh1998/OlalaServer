using APIProject.Common.Models.StorageExport;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Utils;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class StorageExportRepository : BaseRepository<StorageExport>, IStorageExportRepository
    {
        public StorageExportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IPagedList<StorageExportModel>> GetStorageExports(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fromDate = Util.ConvertFromDate(FromDate);
                    var toDate = Util.ConvertToDate(ToDate);
                    var model = (from se in DbContext.StorageExports
                                 where se.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(SearchKey) ? se.Code.Contains(SearchKey) : true)
                                  && (StorageID.HasValue ? se.StorageID.Equals(StorageID) : true)
                                  && (fromDate.HasValue ? se.ExportDate >= fromDate : true)
                                  && (toDate.HasValue ? se.ExportDate <= toDate : true)
                                 orderby se.ID descending
                                 select new StorageExportModel
                                 {
                                     ID = se.ID,
                                     Code = se.Code,
                                     ExportDate = se.ExportDate,
                                     Storage = se.Storage.Name,
                                     TotalPrice = se.TotalPrice,
                                     Customer = se.Customer,
                                     Note = se.Note,
                                     NumberCar = se.NumberCar,
                                     Province = se.Province.Name,
                                     Reason = se.Reason,
                                     ReceiverName = se.ReceiverName,
                                     Condition = se.Condition,
                                     TotalWeight = se.TotalWeight
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<StorageExportDetailModel> GetStorageExportDetail(int ID)
        {
            try
            {
                var model = await (from se in DbContext.StorageExports
                                   where se.IsActive.Equals(SystemParam.ACTIVE) && se.ID.Equals(ID)
                                   select new StorageExportDetailModel
                                   {
                                       ID = se.ID,
                                       Code = se.Code,
                                       ExportDate = se.ExportDate,
                                       Storage = se.Storage.Name,
                                       TotalPrice = se.TotalPrice,
                                       TotalWeight = se.TotalWeight,
                                       Condition = se.Condition,
                                       ReceiverName = se.ReceiverName,
                                       NumberCar = se.NumberCar,
                                       Note = se.Note,
                                       Reason = se.Reason,
                                       Customer = se.Customer,
                                       Province = se.Province.Name,
                                       StorageExportProducts = (from sed in DbContext.StorageExportDetails
                                                                join ps in DbContext.ProductStorages on sed.ProductStorageID equals ps.ID
                                                                join p in DbContext.Products on ps.ProductID equals p.ID
                                                                where sed.StorageExportID.Equals(se.ID)
                                                                select new StorageExportProductModel
                                                                {
                                                                    ID = sed.ID,
                                                                    ExpiredDate = ps.ExpiredDate,
                                                                    ManufactureDate = ps.ManufactureDate,
                                                                    Code = p.Code,
                                                                    Name = p.Name,
                                                                    Supplier = ps.Supplier,
                                                                    Quantity = sed.Quantity,
                                                                    LotNo = ps.LotNo,
                                                                    TotalPrice = sed.TotalPrice,
                                                                    TotalWeight = sed.TotalWeight,
                                                                    Unit = p.Unit,
                                                                    NetWeight = p.NetWeight,
                                                                    Price = sed.Price
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
