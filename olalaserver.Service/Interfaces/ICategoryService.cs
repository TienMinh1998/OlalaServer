using APIProject.Common.Models.Category;
using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface ICategoryService : IServices<Category>
    {
      Task<JsonResultModel> Create(CreateCategoryModel input);
      Task<JsonResultModel> Update(UpdateCategoryModel input);
      Task<JsonResultModel> Delete(int ID);
      Task<JsonResultModel> GetCategories(int page, int limit);
      Task<JsonResultModel> GetCategories();
    }
}
