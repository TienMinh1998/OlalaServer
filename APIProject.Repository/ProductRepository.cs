using APIProject.Repository;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using APIProject.Service.Utils;
using Microsoft.EntityFrameworkCore;
using APIProject.Common.Models.Product;

namespace APIProject.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IPagedList<ProductWebModel>> GetProducts(int Page, int Limit, string SearchKey, int? Status, string FromDate, string ToDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fromDate = Util.ConvertFromDate(FromDate);
                    var toDate = Util.ConvertToDate(ToDate);
                    var model = (from p in DbContext.Products
                                 where p.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(SearchKey) ? (p.Code.Contains(SearchKey) || p.Name.Contains(SearchKey)) : true)
                                  && (Status.HasValue ? p.Status.Equals(Status) : true)
                                  && (fromDate.HasValue ? p.CreatedDate >= fromDate : true)
                                  && (toDate.HasValue ? p.CreatedDate <= toDate : true)
                                 orderby p.ID descending
                                 select new ProductWebModel
                                 {
                                     ID = p.ID,
                                     Name = p.Name,
                                     Code = p.Code,
                                     Status = p.Status,
                                     Unit = p.Unit,
                                     Size = p.Size,
                                     NetWeight = p.NetWeight,
                                     MinQuantityStorage = p.MinQuantityStorage,
                                     Quantity = DbContext.ProductStorages.Where(x => x.ProductID.Equals(p.ID) && x.IsActive.Equals(SystemParam.ACTIVE)).Sum(x => x.Quantity),
                                     Origin = p.Origin,
                                     CreatedDate = p.CreatedDate,
                                     Type = p.Type
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IPagedList<ProductAppModel>> GetProducts(int Page, int Limit, string SearchKey, int? Type)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from p in DbContext.Products
                                 where p.IsActive.Equals(SystemParam.ACTIVE) && p.Status.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(SearchKey) ? p.Name.Contains(SearchKey) : true)
                                 && (Type.HasValue ? p.Type.Equals(Type) : true)
                                 select new
                                 {
                                     ID = p.ID,
                                     Name = p.Name,
                                     Type = p.Type,
                                     Unit = p.Unit,
                                     ListImage = p.ProductImages
                                 }).AsEnumerable().Select(x => new ProductAppModel
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                     ImageUrl = x.ListImage.Select(pi => pi.ImageUrl).FirstOrDefault(),
                                     Type = x.Type,
                                     Unit = x.Unit,
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductDetailAppModel> GetProductDetail(int ID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from p in DbContext.Products
                                 where p.IsActive.Equals(SystemParam.ACTIVE) && p.Status.Equals(SystemParam.ACTIVE) && p.ID.Equals(ID)
                                 select new
                                 {
                                     ID = p.ID,
                                     Name = p.Name,
                                     LatinName = p.LatinName,
                                     EnglishName = p.EnglishName,
                                     Type = p.Type,
                                     Unit = p.Unit,
                                     CategoryID = p.CategoryID,
                                     Code = p.Code,
                                     Description = p.Description,
                                     Ingredient = p.Ingredient,
                                     Origin = p.Origin,
                                     Size = p.Size,
                                     StorageTemperature = p.StorageTemperature,
                                     MinQuantityStorage = p.MinQuantityStorage,
                                     Usage = p.Usage,
                                     Weight = p.NetWeight,
                                     ListProductImage = p.ProductImages,
                                 }).AsEnumerable().Select(x => new ProductDetailAppModel
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                     LatinName = x.LatinName,
                                     EnglishName = x.EnglishName,
                                     Type = x.Type,
                                     Unit = x.Unit,
                                     CategoryID = x.CategoryID,
                                     Code = x.Code,
                                     Description = x.Description,
                                     Ingredient = x.Ingredient,
                                     Origin = x.Origin,
                                     Size = x.Size,
                                     StorageTemperature = x.StorageTemperature,
                                     Usage = x.Usage,
                                     MinQuantityStorage = x.MinQuantityStorage,
                                     NetWeight = x.Weight,
                                     ListImage = x.ListProductImage.Select(pi => pi.ImageUrl).ToList(),
                                     ListProductRelative = (from p in DbContext.Products
                                                            where p.CategoryID.Equals(x.CategoryID) && p.IsActive.Equals(SystemParam.ACTIVE) && p.Status.Equals(SystemParam.ACTIVE)
                                                            select new
                                                            {
                                                                ID = p.ID,
                                                                Name = p.Name,
                                                                Type = p.Type,
                                                                Unit = p.Unit,
                                                                ListProductImage = p.ProductImages,
                                                            }).AsEnumerable().Select(pr => new ProductAppModel
                                                            {
                                                                ID = pr.ID,
                                                                Name = pr.Name,
                                                                Type = pr.Type,
                                                                Unit = pr.Unit,
                                                                ImageUrl = pr.ListProductImage.Select(pi => pi.ImageUrl).FirstOrDefault()
                                                            }).Take(SystemParam.LIMIT_DEFAULT).ToList()
                                 }).FirstOrDefault();
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProductDetailAppModel> GetProductDetail(int ID, int CustomerType)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from p in DbContext.Products
                                 join pit in DbContext.ProductItems on p.ID equals pit.ProductID
                                 where p.IsActive.Equals(SystemParam.ACTIVE) && pit.CustomerTypeID.Equals(CustomerType) && p.Status.Equals(SystemParam.ACTIVE) && p.ID.Equals(ID)
                                 select new
                                 {
                                     ID = p.ID,
                                     Name = p.Name,
                                     LatinName = p.LatinName,
                                     EnglishName = p.EnglishName,
                                     Type = p.Type,
                                     Unit = p.Unit,
                                     CategoryID = p.CategoryID,
                                     Code = p.Code,
                                     Description = p.Description,
                                     Ingredient = p.Ingredient,
                                     Origin = p.Origin,
                                     Size = p.Size,
                                     StorageTemperature = p.StorageTemperature,
                                     Usage = p.Usage,
                                     Weight = p.NetWeight,
                                     ListProductImage = p.ProductImages,
                                     OriginalPrice = pit.OriginalPrice,
                                     Price = pit.Price
                                 }).AsEnumerable().Select(x => new ProductDetailAppModel
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                     LatinName = x.LatinName,
                                     EnglishName = x.EnglishName,
                                     Type = x.Type,
                                     Unit = x.Unit,
                                     CategoryID = x.CategoryID,
                                     Code = x.Code,
                                     Description = x.Description,
                                     Ingredient = x.Ingredient,
                                     Origin = x.Origin,
                                     Size = x.Size,
                                     StorageTemperature = x.StorageTemperature,
                                     Usage = x.Usage,
                                     NetWeight = x.Weight,
                                     ListImage = x.ListProductImage.Select(pi => pi.ImageUrl).ToList(),
                                     OriginalPrice = x.OriginalPrice,
                                     Price = x.Price,
                                     ListProductRelative = (from p in DbContext.Products
                                                            join pit in DbContext.ProductItems on p.ID equals pit.ProductID
                                                            where p.CategoryID.Equals(x.CategoryID) && x.CategoryID.HasValue && p.IsActive.Equals(SystemParam.ACTIVE) && pit.CustomerTypeID.Equals(CustomerType) && p.Status.Equals(SystemParam.ACTIVE) && !p.ID.Equals(x.ID)
                                                            select new
                                                            {
                                                                ID = p.ID,
                                                                Name = p.Name,
                                                                Type = p.Type,
                                                                Unit = p.Unit,
                                                                Price = pit.Price,
                                                                OriginalPrice = pit.OriginalPrice,
                                                                ListProductImage = p.ProductImages,
                                                            }).AsEnumerable().Select(pr => new ProductAppModel
                                                            {
                                                                ID = pr.ID,
                                                                Name = pr.Name,
                                                                Price = pr.Price,
                                                                OriginalPrice = pr.OriginalPrice,
                                                                Type = pr.Type,
                                                                Unit = pr.Unit,
                                                                ImageUrl = pr.ListProductImage.Select(pi => pi.ImageUrl).FirstOrDefault()
                                                            }).Take(SystemParam.LIMIT_DEFAULT).ToList()
                                 }).FirstOrDefault();
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<ProductAppModel>> GetProducts(int Page, int Limit, int CustomerType, string SearchKey, int? Type, int? SortPriceType, int? CategoryID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var query = (from p in DbContext.Products
                                 join pit in DbContext.ProductItems on p.ID equals pit.ProductID
                                 where p.IsActive.Equals(SystemParam.ACTIVE) && pit.CustomerTypeID.Equals(CustomerType) && p.Status.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(SearchKey) ? p.Name.Contains(SearchKey) : true)
                                 && (Type.HasValue ? p.Type.Equals(Type) : true)
                                 && (CategoryID.HasValue ? p.CategoryID.Equals(CategoryID) : true)
                                 select new
                                 {
                                     ID = p.ID,
                                     Name = p.Name,
                                     Type = p.Type,
                                     Unit = p.Unit,
                                     ListProductImage = p.ProductImages,
                                     OriginalPrice = pit.OriginalPrice,
                                     Price = pit.Price
                                 }).AsEnumerable().Select(x => new ProductAppModel
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                     Type = x.Type,
                                     Unit = x.Unit,
                                     ImageUrl = x.ListProductImage.Select(pi => pi.ImageUrl).FirstOrDefault(),
                                     OriginalPrice = x.OriginalPrice,
                                     Price = x.Price
                                 }).AsQueryable();
                    if (SortPriceType.HasValue)
                    {
                        if (SortPriceType.Equals(SystemParam.SORT_ASCENDING))
                        {
                            return query.OrderBy(x => x.Price).ToPagedList(Page, Limit);
                        }
                        else
                        {
                            return query.OrderByDescending(x => x.Price).ToPagedList(Page, Limit);
                        }
                    }
                    else
                    {
                        return query.OrderByDescending(x => x.ID).ToPagedList(Page, Limit);
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountProduct()
        {
            try
            {
                return await DbContext.Products.Where(x => x.IsActive.Equals(SystemParam.ACTIVE)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
