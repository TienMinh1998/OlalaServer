using APIProject.Common.Models.News;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Home;
using APIProject.Service.Models.News;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class HomeService : IHomeService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        private readonly INewsRepository _newsRepository;

        public HomeService(IProductRepository productRepository, IMapper mapper, IHub sentryHub, INewsRepository newsRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _newsRepository = newsRepository;
        }

        public async Task<JsonResultModel> GetHome(int? CusType)
        {
            try
            {
                // lấy danh sách banner 
                var news = await _newsRepository.GetAllAsync(x => x.TypeNews.Equals(SystemParam.TYPE_BANNER) && x.Type.Equals(SystemParam.STATUS_POST_NEWS) && x.IsActive.Equals(SystemParam.ACTIVE) && !string.IsNullOrEmpty(x.UrlImage), source => source.OrderByDescending(x => x.CreatedDate));
                var listBanner = _mapper.Map<List<NewsModel>>(news);

                if (CusType.HasValue)
                {
                    var model = new HomeScreenModel
                    {
                        ListBanner = listBanner,
                        ListHotProduct = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, CusType.GetValueOrDefault(), null, SystemParam.PRODUCT_TYPE_HOT, null, null),
                        ListProductTrend = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, CusType.GetValueOrDefault(), null, SystemParam.PRODUCT_TYPE_TREND, null, null),
                        ListSaleProduct = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, CusType.GetValueOrDefault(), null, SystemParam.PRODUCT_TYPE_SALE, null, null),
                        ListProduct = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, CusType.GetValueOrDefault(), null, null, null, null),

                    };
                    return JsonResponse.Success(model);
                }
                else
                {
                    var model = new HomeScreenModel
                    {
                        ListBanner = listBanner,
                        ListHotProduct = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, null, SystemParam.PRODUCT_TYPE_HOT),
                        ListProductTrend = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, null, SystemParam.PRODUCT_TYPE_TREND),
                        ListSaleProduct = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, null, SystemParam.PRODUCT_TYPE_SALE),
                        ListProduct = await _productRepository.GetProducts(SystemParam.PAGE_DEFAULT, SystemParam.LIMIT_DEFAULT, null, null),
                    };
                    return JsonResponse.Success(model);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetHomeProduct(int Page, int Limit, int? CusType)
        {
            try
            {
                if (CusType.HasValue)
                {
                    var model = await _productRepository.GetProducts(Page, Limit, CusType.GetValueOrDefault(), null, null, null, null);
                    return JsonResponse.Success(model);
                }
                else
                {
                    var model = await _productRepository.GetProducts(Page, Limit, null, null);
                    return JsonResponse.Success(model);

                }

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
