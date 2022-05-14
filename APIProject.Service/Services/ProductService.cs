

using APIProject.Service.Models;
using APIProject.Service.Utils;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Models.Home;
using static APIProject.Service.Utils.SystemParam;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using APIProject.Common.Models.Product;
using Sentry;

namespace APIProject.Service.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductItemRepository _productItemRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public ProductService(IProductRepository productRepository, IMapper mapper, IProductItemRepository productItemRepository, IProductImageRepository productImageRepository, IHub sentryHub, IPushNotificationService pushNotificationService, ICustomerRepository customerRepository) : base(productRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productItemRepository = productItemRepository;
            _productImageRepository = productImageRepository;
            _sentryHub = sentryHub;
            _pushNotificationService = pushNotificationService;
            _customerRepository = customerRepository;
        }
        public async Task<JsonResultModel> GetProducts(int Page, int Limit, string SearchKey, int? Status, string FromDate, string ToDate)
        {
            try
            {
                var model = await _productRepository.GetProducts(Page, Limit, SearchKey, Status, FromDate, ToDate);
                var product = _mapper.Map<List<ProductWebModel>>(model);
                var data = new DataPagedListModel
                {
                    Page = Page,
                    Limit = Limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = product
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetProducts(int Page, int Limit, string SearchKey, int? CustomerType, int? Type, int? SortPriceType, int? CategoryID)
        {
            try
            {
                if (CustomerType.HasValue)
                {
                    var model = await _productRepository.GetProducts(Page, Limit, CustomerType.GetValueOrDefault(), SearchKey, Type, SortPriceType, CategoryID);
                    var product = _mapper.Map<List<ProductAppModel>>(model);
                    return JsonResponse.Success(product);
                }
                else
                {
                    var model = await _productRepository.GetProducts(Page, Limit, SearchKey, Type);
                    var product = _mapper.Map<List<ProductAppModel>>(model);
                    return JsonResponse.Success(product);
                }

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        // Lấy chi tiết sản phẩm trên Web
        public async Task<JsonResultModel> GetProductDetail(int ID)
        {
            try
            {
                var model = await _productRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE), null, source => source.Include(x => x.ProductImages).Include(x => x.ProductItems));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PRODUCT_NOT_EXIST, SystemParam.MESSAGE_PRODUCT_NOT_EXIST);
                }
                var product = _mapper.Map<ProductDetailWebModel>(model);
                return JsonResponse.Success(product);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }


        }
        public async Task<JsonResultModel> CreateProduct(CreateProductModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Code) || String.IsNullOrEmpty(input.Name) || String.IsNullOrEmpty(input.Unit) || input.NetWeight <= 0 || input.ListImage.Count <= 0)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CREATE_PRODUCT_FIELDS_INVALID, SystemParam.MESSAGE_CREATE_PRODUCT_FIELDS_INVALID);
                }
                var listCustomerType = new List<int>();
                foreach (var item in input.ListProductItem)
                {
                    if (item.CustomerType <= 0 || item.Price <= 0)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_CREATE_PRODUCT_FIELDS_INVALID, SystemParam.MESSAGE_CREATE_PRODUCT_FIELDS_INVALID);
                    }
                    if (!item.OriginalPrice.HasValue)
                    {
                        item.OriginalPrice = item.Price;
                    }
                    listCustomerType.Add(item.CustomerType);
                }
                var checkProduct = await _productRepository.GetFirstOrDefaultAsync(x => x.Code.Equals(input.Code) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (checkProduct != null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CREATE_PRODUCT_DUPLICATE_CODE, SystemParam.MESSAGE_CREATE_PRODUCT_DUPLICATE_CODE);
                }
                var product = _mapper.Map<Product>(input);
                var createProduct = await _productRepository.AddAsync(product);
                foreach (var item in input.ListImage)
                {
                    var productImage = new ProductImage
                    {
                        ImageUrl = item,
                        ProductID = createProduct.ID,
                        Product = createProduct
                    };
                    await _productImageRepository.AddAsync(productImage);
                }
                if (input.isNotify == SystemParam.ACTIVE)
                {
                    var listCus = await _customerRepository.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && listCustomerType.Contains(x.CustomerTypeID));
                    await _pushNotificationService.PushNotification(listCus, SystemParam.NOTIFICATION_TYPE_PRODUCT_NEW, string.Format(SystemParam.NOTIFICATION_TYPE_PRODUCT_NEW_STR, product.Name), null, null, product.ID);
                }
                return JsonResponse.Success(_mapper.Map<ProductDetailWebModel>(createProduct));
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> UpdateProduct(UpdateProductModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.Name) || String.IsNullOrEmpty(input.Unit) || input.NetWeight <= 0 || input.ListImage.Count <= 0)
                {
                    return JsonResponse.Error(SystemParam.ERROR_CREATE_PRODUCT_FIELDS_INVALID, SystemParam.MESSAGE_CREATE_PRODUCT_FIELDS_INVALID);
                }
                var model = await _productRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PRODUCT_NOT_EXIST, SystemParam.MESSAGE_PRODUCT_NOT_EXIST);
                }
                foreach (var item in input.ListProductItemCreate)
                {
                    if (item.CustomerType <= 0 || item.Price <= 0)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_CREATE_PRODUCT_FIELDS_INVALID, SystemParam.MESSAGE_CREATE_PRODUCT_FIELDS_INVALID);
                    }
                    if (!item.OriginalPrice.HasValue)
                    {
                        item.OriginalPrice = item.Price;
                    }
                }
                foreach (var item in input.ListProductItemUpdate)
                {
                    if (item.CustomerType <= 0 || item.Price <= 0)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_CREATE_PRODUCT_FIELDS_INVALID, SystemParam.MESSAGE_CREATE_PRODUCT_FIELDS_INVALID);
                    }
                    if (!item.OriginalPrice.HasValue)
                    {
                        item.OriginalPrice = item.Price;
                    }
                }
                var productUpdate = _mapper.Map<Product>(input);
                productUpdate.CreatedDate = model.CreatedDate;
                var product = await _productRepository.UpdateAsync(productUpdate);
                var oldImages = await _productImageRepository.GetAllAsync(x => x.ProductID.Equals(productUpdate.ID));
                foreach (var item in oldImages)
                {
                    await _productImageRepository.DeleteAsync(item);
                }
                foreach (var item in input.ListImage)
                {
                    var productImage = new ProductImage
                    {
                        ImageUrl = item,
                        ProductID = productUpdate.ID,
                        Product = productUpdate
                    };
                    await _productImageRepository.AddAsync(productImage);
                }
                foreach (var item in input.ListProductItemCreate)
                {
                    var productItem = new ProductItem
                    {
                        Price = item.Price,
                        OriginalPrice = item.OriginalPrice.GetValueOrDefault(),
                        CustomerTypeID = item.CustomerType,
                        Product = productUpdate,
                        ProductID = productUpdate.ID
                    };
                    await _productItemRepository.AddAsync(productItem);
                }
                foreach (var item in input.ListProductItemUpdate)
                {
                    var productItem = await _productItemRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(item.ID));
                    productItem.Price = item.Price;
                    productItem.OriginalPrice = item.OriginalPrice.GetValueOrDefault();
                    productItem.CustomerTypeID = item.CustomerType;
                    await _productItemRepository.UpdateAsync(productItem);
                }
                foreach (var item in input.ListProductItemDelete)
                {
                    var productItem = new ProductItem
                    {
                        ID = item.ID,
                    };
                    await _productItemRepository.DeleteAsync(productItem);
                }
                return JsonResponse.Success(_mapper.Map<ProductDetailWebModel>(product));
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteProduct(int ID)
        {
            try
            {
                var model = await _productRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PRODUCT_NOT_EXIST, SystemParam.MESSAGE_PRODUCT_NOT_EXIST);
                }
                model.IsActive = SystemParam.ACTIVE_FALSE;
                await _productRepository.UpdateAsync(model);

                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        // Lấy chi tiết sản phẩm trên App
        public async Task<JsonResultModel> GetProductDetail(int ID, int? CustomerType)
        {
            try
            {
                var model = CustomerType.HasValue ? await _productRepository.GetProductDetail(ID, CustomerType.GetValueOrDefault()) : await _productRepository.GetProductDetail(ID);
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PRODUCT_NOT_EXIST, SystemParam.MESSAGE_PRODUCT_NOT_EXIST);
                }
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateProductStatus(int ID)
        {
            try
            {
                var model = await _productRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (model == null)
                {
                    return JsonResponse.Error(SystemParam.ERROR_PRODUCT_NOT_EXIST, SystemParam.MESSAGE_PRODUCT_NOT_EXIST);
                }
                model.Status = model.Status.Equals(SystemParam.ACTIVE) ? SystemParam.ACTIVE_FALSE : SystemParam.ACTIVE;
                await _productRepository.UpdateAsync(model);

                return JsonResponse.Success(model.ID);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}

