using APIProject.Common.Models.Category;
using APIProject.Common.Models.Product;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IPagedList<CategoryModel>> GetCategories(int Page, int Limit);
    }
}
