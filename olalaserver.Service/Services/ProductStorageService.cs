using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Utils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Sentry;
using APIProject.Service.Models;

namespace APIProject.Service.Services
{
    public class ProductStorageService : BaseService<ProductStorage>, IProductStorageService
    {
        private readonly IProductStorageRepository _ProductStorageRepository;
        private readonly IProductStorageHistoryRepository _ProductStorageHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public ProductStorageService(IProductStorageRepository ProductStorageRepository, IMapper mapper, IHub sentryHub, IProductStorageHistoryRepository productStorageHistoryRepository) : base(ProductStorageRepository)
        {
            _ProductStorageRepository = ProductStorageRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _ProductStorageHistoryRepository = productStorageHistoryRepository;
        }

        public async Task<JsonResultModel> GetListProductStorage(int Page, int Limit, string SearchKey, int? StorageID)
        {
            try
            {
                var model = await _ProductStorageRepository.GetProductStorages(Page, Limit, SearchKey, StorageID);
                var data = new DataPagedListModel
                {
                    Page = Page,
                    Limit = Limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetProductStorageDetail(int ProductStorageID)
        {
            try
            {
                var productStorage = await _ProductStorageRepository.GetProductStorageDetail(ProductStorageID);
                if (productStorage == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PRODUCT_STORAGE_NOT_FOUND, SystemParam.MESSAGE_PRODUCT_STORAGE_NOT_FOUND);
                }

                return JsonResponse.Success(productStorage);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetProductStorageHistory(int Page, int Limit, int ProductStorageID)
        {
            try
            {
                var model = await _ProductStorageHistoryRepository.GetProductStorageHistory(Page, Limit, ProductStorageID);
                var data = new DataPagedListModel
                {
                    Page = Page,
                    Limit = Limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<int?> GetProductQuantity(int ProductID)
        {
            try
            {
                var productStorages = await _ProductStorageRepository.GetAllAsync(x => x.ProductID.Equals(ProductID) && x.Storage.Status.Equals(SystemParam.ACTIVE));
                if (productStorages.Count == 0)
                {
                    return null;
                }
                else
                {
                    var quantity = productStorages.Sum(x => x.Quantity);
                    return quantity;
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return null;
            }
        }

        public async Task<JsonResultModel> GetListProductStorageByProduct(int Page, int Limit, string SearchKey)
        {
            try
            {
                var model = await _ProductStorageRepository.GetProductStoragesByProduct(Page, Limit, SearchKey);
                var data = new DataPagedListModel
                {
                    Page = Page,
                    Limit = Limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
