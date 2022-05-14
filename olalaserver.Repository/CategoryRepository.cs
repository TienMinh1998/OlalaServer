using APIProject.Common.Models.Category;
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

namespace APIProject.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IPagedList<CategoryModel>> GetCategories(int Page, int Limit)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from c in DbContext.Categories
                                 where c.IsActive.Equals(SystemParam.ACTIVE)
                                 orderby c.CreatedDate descending
                                 select new CategoryModel
                                 {
                                     ID = c.ID,
                                     Name = c.Name,
                                     Status = c.Status,
                                     createdate = c.CreatedDate
                                 }).ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from c in DbContext.Categories
                                 where c.IsActive.Equals(SystemParam.ACTIVE)
                                 orderby c.CreatedDate descending
                                 select new CategoryModel
                                 {
                                     ID = c.ID,
                                     Name = c.Name,
                                     Status = c.Status,
                                     createdate = c.CreatedDate
                                 }).ToList();
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
