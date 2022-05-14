using APIProject.Common.Models.ProductStorage;
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
    public class ProductStorageRepository : BaseRepository<ProductStorage>, IProductStorageRepository
    {
        public ProductStorageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ProductStorageDetailModel> GetProductStorageDetail(int ProductStorageID)
        {
            try
            {
                var model = await (from ps in DbContext.ProductStorages
                                   where ps.IsActive.Equals(SystemParam.ACTIVE) && ps.ID.Equals(ProductStorageID)
                                   select new ProductStorageDetailModel
                                   {
                                       ID = ps.ID,
                                       ProductName = ps.Product.Name,
                                       ProductCode = ps.Product.Code,
                                       Storage = ps.Storage.Name,
                                       Quantity = ps.Quantity,
                                       Unit = ps.Product.Unit,
                                       Supplier = ps.Supplier,
                                       ExpiredDate = ps.ExpiredDate,
                                       ManufactureDate = ps.ManufactureDate,
                                       LotNo = ps.LotNo,
                                       NetWeight = ps.Product.NetWeight,
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<ProductStorageModel>> GetProductStorages(int Page, int Limit, string SearchKey, int? StorageID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from ps in DbContext.ProductStorages
                                 where ps.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(SearchKey) ? (ps.Product.Code.Contains(SearchKey) || ps.Product.Name.Contains(SearchKey)) : true)
                                  && (StorageID.HasValue ? ps.StorageID.Equals(StorageID) : true)
                                 orderby ps.ExpiredDate 
                                 select new ProductStorageModel
                                 {
                                     ID = ps.ID,
                                     ProductName = ps.Product.Name,
                                     ProductCode = ps.Product.Code,
                                     Storage = ps.Storage.Name,
                                     Quantity = ps.Quantity,
                                     Unit = ps.Product.Unit,
                                     Size = ps.Product.Size,
                                     Origin = ps.Product.Origin,
                                     Supplier = ps.Supplier,
                                     ExpiredDate = ps.ExpiredDate,
                                     ManufactureDate = ps.ManufactureDate,
                                     LotNo = ps.LotNo,
                                     NetWeight = ps.Product.NetWeight,
                                     TotalWeight = ps.Product.NetWeight * ps.Quantity,
                                     MinQuantityStorage = ps.Product.MinQuantityStorage,
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IPagedList<ProductStorageByProductModel>> GetProductStoragesByProduct(int Page, int Limit, string SearchKey)
        {
            try
            {
                var model = (from p in DbContext.Products
                             where p.IsActive.Equals(SystemParam.ACTIVE)
                             && (!String.IsNullOrEmpty(SearchKey) ? (p.Code.Contains(SearchKey) || p.Name.Contains(SearchKey)) : true)
                             orderby p.ID descending
                             select new ProductStorageByProductModel
                             {
                                 ID = p.ID,
                                 ProductCode = p.Code,
                                 ProductName = p.Name,
                                 NetWeight = p.NetWeight,
                                 Unit = p.Unit,
                                 TotalQuantity = DbContext.ProductStorages.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.ProductID.Equals(p.ID) && x.Quantity > 0).Sum(x => x.Quantity)
                             }).AsEnumerable().Select(x => new ProductStorageByProductModel
                             {
                                 ID = x.ID,
                                 ProductCode = x.ProductCode,
                                 NetWeight = x.NetWeight,
                                 ProductName = x.ProductName,
                                 Unit = x.Unit,
                                 TotalQuantity = x.TotalQuantity,
                                 TotalWeight = x.NetWeight * x.TotalQuantity,
                                 ListStorageQuantity = DbContext.Storages.Select(s => new StorageQuantityModel
                                 {
                                     ID = s.ID,
                                     Name = s.Name,
                                     Quantity = DbContext.ProductStorages.Where(ps => ps.IsActive.Equals(SystemParam.ACTIVE) && ps.ProductID.Equals(x.ID) && ps.StorageID.Equals(s.ID) && ps.Quantity > 0).Sum(x => x.Quantity),
                                 }).AsEnumerable().Select(sq => new StorageQuantityModel
                                 {
                                     ID = sq.ID,
                                     Name = sq.Name,
                                     Quantity = sq.Quantity,
                                     TotalWeight = sq.Quantity * x.NetWeight
                                 }).ToList()
                             }).AsQueryable().ToPagedList(Page, Limit);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
