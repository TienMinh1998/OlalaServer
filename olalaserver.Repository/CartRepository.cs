using APIProject.Common.Models.Cart;
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
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<int> GetCartCount(int CusID)
        {
            try
            {
                var count = await DbContext.Carts.CountAsync(x => x.CustomerID.Equals(CusID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.Type.Equals(SystemParam.TYPE_BASIC_ITEM));
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CartModelDetail>> GetCarts(int cusID, int? Type)
        {
            try
            {
                return await Task.Run(() =>
                  {
                      var lst = (from c in DbContext.Carts
                                 join p in DbContext.ProductItems on c.ProductItemID equals p.ID
                                 where c.CustomerID.Equals(cusID) && c.IsActive.Equals(SystemParam.ACTIVE) && c.Type.Equals(Type)
                                 orderby c.ID descending
                                 select new
                                 {
                                     Price = p.Price,
                                     ProductID = p.ProductID,
                                     Quantity = c.Quantity,
                                     ID = c.ID,
                                     IsActice = c.IsActive,
                                     CreateDate = c.CreatedDate,
                                     CustomerID = c.CustomerID,
                                     Unit = p.Product.Unit,
                                     SumPrice = c.SumPrice,
                                     Name = p.Product.Name,
                                     Status = p.Product.Status
                                 })
                               .AsEnumerable().Where(res => res.Status == SystemParam.ACTIVE).Select(x => new CartModelDetail
                               {
                                   ImageUrl = DbContext.ProductImages.Where(i => i.ProductID == x.ProductID).FirstOrDefault().ImageUrl,
                                   Unit = x.Unit,
                                   CartID = x.ID,
                                   Price = x.Price,
                                   Quantity = x.Quantity,
                                   SumPrice = x.SumPrice,
                                   ProductName = x.Name
                               }).ToList();
                      return lst;
                  });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
