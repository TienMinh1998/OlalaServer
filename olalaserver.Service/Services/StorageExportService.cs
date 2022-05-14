using APIProject.Common.Models.StorageExport;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class StorageExportService : IStorageExportService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IStorageExportRepository _storageExportRepository;
        private readonly IStorageExportDetailRepository _storageExportDetailRepository;
        private readonly IProductStorageRepository _productStorageRepository;
        private readonly IProductStorageService _productStorageService;
        private readonly IProductRepository _productRepository;
        private readonly IProductStorageHistoryRepository _productStorageHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        private readonly ISocketService _socketService;

        public StorageExportService(IStorageExportRepository StorageExportRepository, IMapper mapper, IHub sentryHub, IProductStorageRepository productStorageRepository, IProductStorageHistoryRepository productStorageHistoryRepository, IStorageExportDetailRepository storageExportDetailRepository, IProductRepository productRepository, IStorageRepository storageRepository, IProductStorageService productStorageService, ISocketService socketService)
        {
            _storageExportRepository = StorageExportRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _productStorageRepository = productStorageRepository;
            _productStorageHistoryRepository = productStorageHistoryRepository;
            _storageExportDetailRepository = storageExportDetailRepository;
            _productRepository = productRepository;
            _storageRepository = storageRepository;
            _productStorageService = productStorageService;
            _socketService = socketService;
        }
        public async Task<JsonResultModel> GetListStorageExport(int Page, int Limit, string SearchKey, int? StorageID, string FromDate, string ToDate)
        {
            try
            {
                var model = await _storageExportRepository.GetStorageExports(Page, Limit, SearchKey, StorageID, FromDate, ToDate);
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

        public async Task<JsonResultModel> GetStorageExportDetail(int ID)
        {
            try
            {
                var model = await _storageExportRepository.GetStorageExportDetail(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_STORAGE_EXPORT_NOT_FOUND, SystemParam.MESSAGE_STORAGE_EXPORT_NOT_FOUND);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> ExportStorage(CreateStorageExportModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var storageIDs = new List<int>();
                    var listProductID = new List<int>();
                    foreach (var item in input.StorageExportProducts)
                    {
                        storageIDs.Add(item.StorageID);
                    }
                    storageIDs = storageIDs.Distinct().ToList();
                    foreach (var item in storageIDs)
                    {
                        var storage = await _storageRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item));
                        if (storage == null)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_STORAGE_NOT_FOUND, SystemParam.MESSAGE_STORAGE_NOT_FOUND);
                        }
                        long totalPrice = 0;
                        double totalWeight = 0;
                        var storageExport = new StorageExport
                        {
                            Code = Util.GenerateCode("XK"),
                            Note = input.Note,
                            StorageID = item,
                            NumberCar = input.NumberCar,
                            ProvinceID = input.ProvinceID,
                            Condition = input.Condition,
                            Reason = input.Reason,
                            ReceiverName = input.ReceiverName,
                            Customer = input.Customer,
                            ExportDate = Util.ConvertDate(input.ExportDate).GetValueOrDefault(),
                        };
                        await _storageExportRepository.AddAsync(storageExport);
                        foreach (var subItem in input.StorageExportProducts)
                        {

                            if (subItem.StorageID.Equals(item))
                            {
                                var productStorage = await _productStorageRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(subItem.ProductStorageID) && x.IsActive.Equals(SystemParam.ACTIVE), null, source => source.Include(x => x.Product));
                                if (productStorage == null)
                                {
                                    return JsonResponse.Error(SystemParam.ERROR_STORAGE_EXPORT_PRODUCT_NOT_FOUND, SystemParam.MESSAGE_STORAGE_EXPORT_PRODUCT_NOT_FOUND);
                                }
                                if (productStorage.Quantity < subItem.Quantity)
                                {
                                    return JsonResponse.Error(SystemParam.ERROR_STORAGE_EXPORT_PRODUCT_EXCEED_QUANTITY, SystemParam.MESSAGE_STORAGE_EXPORT_PRODUCT_EXCEED_QUANTITY);
                                }
                                listProductID.Add(productStorage.ProductID);
                                productStorage.Quantity -= subItem.Quantity;
                                await _productStorageRepository.UpdateAsync(productStorage);
                                var storageExportDetail = new StorageExportDetail
                                {
                                    ProductStorageID = subItem.ProductStorageID,
                                    StorageExportID = storageExport.ID,
                                    Price = subItem.Price,
                                    Quantity = subItem.Quantity,
                                    TotalPrice = subItem.Price * subItem.Quantity,
                                    TotalWeight = productStorage.Product.NetWeight * subItem.Quantity
                                };
                                await _storageExportDetailRepository.AddAsync(storageExportDetail);
                                var productStorageHistory = new ProductStorageHistory
                                {
                                    ProductStorageID = productStorage.ID,
                                    Balance = productStorage.Quantity,
                                    Quantity = subItem.Quantity,
                                    Code = storageExport.Code,
                                    Type = SystemParam.TYPE_PRODUCT_STORAGE_HISTORY_EXPORT,
                                    Price = subItem.Price
                                };
                                await _productStorageHistoryRepository.AddAsync(productStorageHistory);
                                totalPrice += storageExportDetail.TotalPrice;
                                totalWeight += storageExportDetail.TotalWeight;
                            }
                        }
                        storageExport.TotalWeight = totalWeight;
                        storageExport.TotalPrice = totalPrice;
                        await _storageExportRepository.UpdateAsync(storageExport);
                    }
                    // Check số lượng tồn kho sản phẩm sau khi xuất kho
                    listProductID = listProductID.Distinct().ToList();
                    foreach(var productID in listProductID)
                    {
                        var productQty = await _productStorageService.GetProductQuantity(productID);
                        var product = await _productRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(productID));
                        if(productQty.GetValueOrDefault() < product.MinQuantityStorage)
                        {
                            await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING, string.Format(SystemParam.NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING_STR, product.Name), null, null, product.Code);
                        }
                    }
                    
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
