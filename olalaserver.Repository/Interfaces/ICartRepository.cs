using APIProject.Common.Models.Cart;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
   public interface ICartRepository : IRepository<Cart>
    {
        Task<List<CartModelDetail>> GetCarts(int CusID,int? Type);
        Task<int> GetCartCount(int CusID);
    }
}
