using APIProject.Common.Models.Cart;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Models.Cart;
using Sentry;

namespace APIProject.Service.Services
{
    public class CartService : BaseService<Cart>, ICartService
    {
        private readonly IProductItemRepository _productItemRepository;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly IHub _sentryHub;
        public CartService(IRepository<Cart> baseReponsitory, IProductItemRepository productItemRepository, IMapper mapper,
            ICartRepository cartRepository, IHub sentryHub) : base(baseReponsitory)
        {
            _cartRepository = cartRepository;
            _productItemRepository = productItemRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
        }
        public async Task<JsonResultModel> AddCart(int CusID, int ProductID, int CusTypeID, int quantity)
        {
            try
            {
                bool checkcart = await CheckCartChange(CusID);
                if (checkcart)
                {

                }
                var productItem = await _productItemRepository.GetFirstOrDefaultAsync(x => x.CustomerTypeID == CusTypeID && x.ProductID == ProductID && x.IsActive.Equals(SystemParam.ACTIVE));
                if (productItem == null) return JsonResponse.Error(SystemParam.ERROR_PRODUCTITEM_NOT_FOUND, SystemParam.MESSAGE_PRODUCTITEM_NOT_FOUND);
                var res = await _cartRepository.GetFirstOrDefaultAsync(x => x.CustomerID == CusID && x.IsActive == SystemParam.ACTIVE && x.ProductItemID == productItem.ID && x.Type.Equals(SystemParam.TYPE_BASIC_ITEM));
                if (res != null)
                {
                    res.Quantity += quantity;
                    res.SumPrice = res.Quantity * res.Price;
                    var cart = await _cartRepository.UpdateAsync(res);
                    return JsonResponse.Success(cart.ID);
                }
                else
                {
                    CartModel model = new CartModel
                    {
                        CustomerID = CusID,
                        ProductItemID = productItem.ID,
                        Price = productItem.Price,
                        Quantity = quantity,
                        SumPrice = productItem.Price * quantity
                    };
                    var entity = _mapper.Map<Cart>(model);
                    var cart = await _cartRepository.AddAsync(entity);
                    return JsonResponse.Success(cart.ID);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<bool> CheckCartChange(int CusID)
        {
            try
            {
                var listCart = await _cartRepository.GetAllAsync(x => x.CustomerID == CusID && x.IsActive.Equals(SystemParam.ACTIVE));
                var check = false;
                foreach (var cart in listCart)
                {
                    var productItem = await _productItemRepository.GetFirstOrDefaultAsync(x => x.ID == cart.ProductItemID && x.IsActive == SystemParam.ACTIVE);
                    if (productItem == null)
                    {
                        cart.IsActive = SystemParam.ACTIVE_FALSE;
                        check = true;
                        continue;
                    }
                    if (cart.Price != productItem.Price)
                    {
                        check = true;
                        cart.Price = productItem.Price;
                        cart.SumPrice = cart.Price * cart.Quantity;
                        await _cartRepository.UpdateAsync(cart);
                    }
                }
                return check;
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return false;
            }

        }


        public async Task<JsonResultModel> GetCart(int CusID, int? Type)
        {
            try
            {
                bool check = await CheckCartChange(CusID);
                var lst = await _cartRepository.GetCarts(CusID, Type.Value);
                if (check == false) return JsonResponse.Success(lst);
                return JsonResponse.Response(SystemParam.SUCCESS, SystemParam.ERROR_CART_UPDATED, SystemParam.MESSAGE_CART_UPDATED, lst);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> DeleteCart(int cusID, DeleteCartModel model)
        {
            try
            {
                int count = 0; // số sản phẩm đã bị thay đổi
                for (int i = 0; i < model.CartID.Count; i++)
                {

                    var item = await _cartRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(model.CartID[i]) && x.IsActive.Equals(SystemParam.ACTIVE) && x.CustomerID.Equals(cusID));
                    if (item == null) continue;
                    if (item != null) count++;
                    item.IsActive = SystemParam.ACTIVE_FALSE;
                    await _cartRepository.UpdateAsync(item);
                }
                if (count == 0) return JsonResponse.Error(SystemParam.ERROR_CART_NOTFOUND, SystemParam.MESSAGE_NOTFOUND_ITEMCART);
                return JsonResponse.Success(count);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> UpdateCart(int cusID, UpdateCartModel model)
        {
            try
            {
                var oldCart = await _cartRepository.GetFirstOrDefaultAsync(x => x.ID == model.CartID && x.IsActive == SystemParam.ACTIVE && x.CustomerID.Equals(cusID));
                if (oldCart == null) return JsonResponse.Error(SystemParam.ERROR_CART_NOTFOUND, SystemParam.MESSAGE_NOTFOUND_ITEMCART);
                if (model.Quantity > SystemParam.QUANTITY_MAX) return JsonResponse.Error(SystemParam.ERROR_CART_QUANTITY_LIMITED, SystemParam.MESSAGE_QUANTITY_ERROR);
                oldCart.Quantity = model.Quantity;
                oldCart.SumPrice = oldCart.Price * model.Quantity;
                var response = await _cartRepository.UpdateAsync(oldCart);
                return JsonResponse.Success(response.ID);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();

            }
        }
        /// <summary>   
        /// Mua ngay sản phẩm
        /// </summary>
        /// <param name="CusID"></param>
        /// <param name="ProductID"></param>
        /// <param name="CusTypeID"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<JsonResultModel> BuyNow(int CusID, int ProductID, int CusTypeID, int quantity)
        {
            try
            {
                var productItem = await _productItemRepository.GetFirstOrDefaultAsync(x => x.CustomerTypeID == CusTypeID && x.ProductID == ProductID && x.IsActive.Equals(SystemParam.ACTIVE));
                if (productItem == null) return JsonResponse.Error(SystemParam.ERROR_PRODUCTITEM_NOT_FOUND, SystemParam.MESSAGE_PRODUCTITEM_NOT_FOUND);
                var res = await _cartRepository.GetAllAsync(x => x.CustomerID == CusID && x.IsActive == SystemParam.ACTIVE && x.ProductItemID == productItem.ID && x.Type == SystemParam.TYPE_BUYNOW);
                for (int i = SystemParam.TYPE_BASIC_ITEM; i < res.Count; i++) await _cartRepository.DeleteAsync(res[i]);
                CartModel model = new CartModel
                {
                    CustomerID = CusID,
                    ProductItemID = productItem.ID,
                    Price = productItem.Price,
                    Quantity = quantity,
                    SumPrice = productItem.Price * quantity,
                    Type = SystemParam.TYPE_BUYNOW
                };
                // Gọi Get Cart : 
                var response = await _cartRepository.AddAsync(_mapper.Map<Cart>(model));
                return JsonResponse.Success(response.ID);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetCartCount(int CusID)
        {
            try
            {
                var count = await _cartRepository.GetCartCount(CusID);
                return JsonResponse.Success(count);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
