using APIProject.Common.Models.Category;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _catagoryService;
        public CategoryController(ICategoryService catagoryService)
        {
            _catagoryService = catagoryService;
        }
        /// <summary>
        /// Lấy danh sách danh mục
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CATEGORY)]
        [HttpGet("GetListCategory")]
        public async Task<JsonResultModel> GetListCategory(int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _catagoryService.GetCategories(Page, Limit);
        }
        /// <summary>
        /// Lấy danh sách danh mục không quyền
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        [HttpGet("GetListCategoryNoPermission")]
        public async Task<JsonResultModel> GetListCategoryNoPermission(int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT)
        {
            return await _catagoryService.GetCategories(Page, Limit);
        }
        /// <summary>
        /// Lấy tất cả sanh sách danh mục Web
        /// </summary>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CATEGORY)]
        [HttpGet("GetAllCategory")]
        public async Task<JsonResultModel> GetAllCategory()
        {
            return await _catagoryService.GetCategories();
        }
        
        /// <summary>
        /// Tạo danh mục
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CATEGORY)]
        [HttpPost("CreateCategory")]
        public async Task<JsonResultModel> CreateCategory([FromBody]CreateCategoryModel input)
        {
            return await _catagoryService.Create(input);
        }

        /// <summary>
        /// Sửa danh mục
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CATEGORY)]
        [HttpPut("UpdateCategory")]
        public async Task<JsonResultModel> UpdateCategory([FromBody]UpdateCategoryModel input)
        {
            return await _catagoryService.Update(input);
        }


        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [AuthorizePermission(SystemParam.PERMISSION_TYPE_CATEGORY)]
        [HttpDelete("DeleteCategory")]
        public async Task<JsonResultModel> DeleteCategory(int ID)
        {
          return await _catagoryService.Delete(ID);
        }
 
       
    }
} 
