using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Services;
using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PagedList.Core;
using APIProject.Common.Models.Category;
using Sentry;

namespace APIProject.Service
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public CategoryService(IRepository<Category> baseReponsitory, ICategoryRepository categoryRepository, IMapper mapper, IHub sentryHub, IProductRepository productRepository) : base(baseReponsitory)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _productRepository = productRepository;
        }

        public async Task<JsonResultModel> Create(CreateCategoryModel input)
        {
            try
            {
                var category = await _categoryRepository.GetFirstOrDefaultAsync(x => x.Name.Equals(input.Name) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (category != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CATEGORY_EXIST, SystemParam.MESSAGE_CATEGORY_EXIST);
                }
                var categoryCreate = _mapper.Map<Category>(input);
                var result = await _categoryRepository.AddAsync(categoryCreate);
                return JsonResponse.Success(result);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> Delete(int ID)
        {
            try
            {
                var rs = await _categoryRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (rs == null) return JsonResponse.Error(SystemParam.ERROR_CATEGORY_NOT_EXIST, SystemParam.MESSAGE_CATEGORY_NOT_EXIST);
                var checkProduct = await _productRepository.GetFirstOrDefaultAsync(x => x.CategoryID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if(checkProduct != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CATEGORY_PRODUCT_STILL_EXIST, SystemParam.MESSAGE_CATEGORY_PRODUCT_STILL_EXIST);
                }
                rs.IsActive = SystemParam.ACTIVE_FALSE;
                await _categoryRepository.UpdateAsync(rs);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetCategories(int page,int limit)
        {
            try
            {
                var model = await _categoryRepository.GetCategories(page, limit);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetCategories()
        {
            try
            {
                var model = await _categoryRepository.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.Status.Equals(SystemParam.ACTIVE));
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> Update(UpdateCategoryModel input)
        {
            try
            {
                var category = await _categoryRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                if (category == null) return JsonResponse.Error(SystemParam.ERROR_CATEGORY_NOT_EXIST, SystemParam.MESSAGE_CATEGORY_NOT_EXIST);
                category.Name = input.Name;
                category.Status = input.Status;
                await _categoryRepository.UpdateAsync(category);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
