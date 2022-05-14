using APIProject.Service.Models;
using APIProject.Service.Interface;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Models.Product;

namespace APIProject.Service.Interfaces
{

    public interface IProductService : IServices<Product>
    {
        Task<JsonResultModel> GetProducts(int Page, int Limit, string SearchKey, int? Status, string FromDate, string ToDate);
        Task<JsonResultModel> CreateProduct(CreateProductModel input);
        Task<JsonResultModel> GetProductDetail(int ID);
        Task<JsonResultModel> GetProductDetail(int ID,int? CustomerType);
        Task<JsonResultModel> UpdateProduct(UpdateProductModel input);
        Task<JsonResultModel> UpdateProductStatus(int ID);
        Task<JsonResultModel> DeleteProduct(int ID);
        Task<JsonResultModel> GetProducts(int Page,int Limit,string SearchKey,int? CustomerType,int? Type,int? SortPriceType,int? CategoryID);
    }
}
