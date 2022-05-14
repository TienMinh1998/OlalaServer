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

namespace APIProject.Repository
{
    public class ProductStorageHistoryRepository : BaseRepository<ProductStorageHistory>, IProductStorageHistoryRepository
    {
        public ProductStorageHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IPagedList<ProductStorageHistoryModel>> GetProductStorageHistory(int Page, int Limit, int ProductStorageID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from psh in DbContext.ProductStorageHistories
                                 where psh.IsActive.Equals(SystemParam.ACTIVE) && psh.ProductStorageID.Equals(ProductStorageID)
                                 orderby psh.ID descending
                                 select new ProductStorageHistoryModel
                                 {
                                     ID = psh.ID,
                                     Code = psh.Code,
                                     Balance = psh.Balance,
                                     Quantity = psh.Quantity,
                                     CreatedDate = psh.CreatedDate,
                                     Type = psh.Type,
                                     Price = psh.Price
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
