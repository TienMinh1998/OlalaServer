using APIProject.Repository.Interfaces;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Models.Product;

namespace APIProject.Repository.Interfaces
{

    public interface IProductRepository : IRepository<Product>
    {
        Task<IPagedList<ProductWebModel>> GetProducts(int Page, int Limit, string SearchKey, int? Status, string FromDate, string ToDate);
        Task<IPagedList<ProductAppModel>> GetProducts(int Page, int Limit,string SearchKey,int? Type);
        Task<IPagedList<ProductAppModel>> GetProducts(int Page, int Limit, int CustomerType, string SearchKey, int? Type, int? SortPriceType,int? CategoryID);
        Task<ProductDetailAppModel> GetProductDetail(int ID, int CustomerType);
        Task<ProductDetailAppModel> GetProductDetail(int ID);
        Task<int> CountProduct();

    }
}
