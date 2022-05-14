using APIProject.Common.Models.StorageImport;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class StorageImportService : IStorageImportService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IStorageImportRepository _storageImportRepository;
        private readonly IStorageImportDetailRepository _storageImportDetailRepository;
        private readonly IProductStorageRepository _productStorageRepository;
        private readonly IProductStorageHistoryRepository _productStorageHistoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;

        public StorageImportService(IStorageImportRepository StorageImportRepository, IMapper mapper, IHub sentryHub, IProductStorageRepository productStorageRepository, IProductStorageHistoryRepository productStorageHistoryRepository, IStorageImportDetailRepository storageImportDetailRepository, IProductRepository productRepository, IStorageRepository storageRepository)
        {
            _storageImportRepository = StorageImportRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _productStorageRepository = productStorageRepository;
            _productStorageHistoryRepository = productStorageHistoryRepository;
            _storageImportDetailRepository = storageImportDetailRepository;
            _productRepository = productRepository;
            _storageRepository = storageRepository;
        }
        public async Task<JsonResultModel> GetListStorageImport(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate)
        {
            try
            {
                var model = await _storageImportRepository.GetStorageImports(Page, Limit, SearchKey, StorageID, FromDate, ToDate);
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

        public async Task<JsonResultModel> GetStorageImportDetail(int ID)
        {
            try
            {
                var model = await _storageImportRepository.GetStorageImportDetail(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_STORAGE_IMPORT_NOT_FOUND, SystemParam.MESSAGE_STORAGE_IMPORT_NOT_FOUND);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ImportStorage(CreateStorageImportModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    long totalPrice = 0;
                    double totalWeight = 0;
                    var storage = await _storageRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.StorageID)); 
                    if(storage == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_STORAGE_NOT_FOUND, SystemParam.MESSAGE_STORAGE_NOT_FOUND);
                    }
                    var storageImport = new StorageImport
                    {
                        StorageID = input.StorageID,
                        ImportDate = Util.ConvertDate(input.ImportDate).GetValueOrDefault(),
                        Code = Util.GenerateCode("NK")
                    };
                    await _storageImportRepository.AddAsync(storageImport);
                    foreach (var item in input.StorageImportProducts)
                    {
                        var product = await _productRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item.ProductID) && x.IsActive.Equals(SystemParam.ACTIVE));
                        if (product == null)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_PRODUCT_NOT_EXIST, SystemParam.MESSAGE_PRODUCT_NOT_EXIST);
                        }
                       
                        var productStorage = new ProductStorage
                        {
                            ProductID = item.ProductID,
                            StorageID = input.StorageID,
                            Quantity = item.Quantity,
                            LotNo = item.LotNo,
                            ExpiredDate = Util.ConvertDate(item.ExpiredDate).GetValueOrDefault(),
                            ManufactureDate = Util.ConvertDate(item.ManufactureDate).GetValueOrDefault(),
                            Supplier = item.Supplier

                        };
                        await _productStorageRepository.AddAsync(productStorage);
                        var productStorageHistory = new ProductStorageHistory
                        {
                            ProductStorageID = productStorage.ID,
                            Balance = item.Quantity,
                            Quantity = item.Quantity,
                            Type = SystemParam.TYPE_PRODUCT_STORAGE_HISTORY_IMPORT,
                            Code = storageImport.Code,
                            Price = item.Price
                        };
                        await _productStorageHistoryRepository.AddAsync(productStorageHistory);
                        var storageImportDetail = new StorageImportDetail
                        {
                            StorageImportID = storageImport.ID,
                            ProductStorageID = productStorage.ID,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            TotalPrice = item.Price * item.Quantity,
                            TotalWeight = product.NetWeight * item.Quantity,
                            Note = item.Note
                        };
                        await _storageImportDetailRepository.AddAsync(storageImportDetail);
                        totalWeight += storageImportDetail.TotalWeight;
                        totalPrice += storageImportDetail.TotalPrice;
                    }
                    storageImport.TotalWeight = totalWeight;
                    storageImport.TotalPrice = totalPrice;
                    await _storageImportRepository.UpdateAsync(storageImport);


                    scope.Complete();

                    return JsonResponse.Success();
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
