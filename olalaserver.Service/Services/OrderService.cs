using APIProject.Common.Models.Order;
using APIProject.Common.Models.StorageExport;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Order;
using APIProject.Service.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace APIProject.Service.Services
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IReceiveAddressRepository _receiveAddressRepository;
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly IMemberPointHistoryRepository _memberPointHistoryRepository;
        private readonly IOrderComplainImageRepository _orderComplainImageRepository;
        private readonly IProductStorageService _productStorageSerivce;
        private readonly IPushNotificationService _pushNotificationSerivce;
        private readonly IHub _sentryHub;
        private readonly IVnpayService _vnpayService;
        private readonly ISocketService _socketService;
        public OrderService(IOrderRepository OrderRepository, ICustomerRepository customerRepository, ICartService cartService, ICartRepository cartRepository, IReceiveAddressRepository receiveAddressRepository, IProductStorageService productStorageService, IOrderHistoryRepository orderHistoryRepository, IOrderDetailRepository orderDetailRepository, IPushNotificationService pushNotificationSerivce, IMemberPointHistoryRepository memberPointHistoryRepository, IOrderComplainImageRepository orderComplainImageRepository, IHub sentryHub, IVnpayService vnpayService, ISocketService socketService) : base(OrderRepository)
        {
            _orderRepository = OrderRepository;
            _customerRepository = customerRepository;
            _cartService = cartService;
            _cartRepository = cartRepository;
            _receiveAddressRepository = receiveAddressRepository;
            _productStorageSerivce = productStorageService;
            _orderHistoryRepository = orderHistoryRepository;
            _orderDetailRepository = orderDetailRepository;
            _pushNotificationSerivce = pushNotificationSerivce;
            _memberPointHistoryRepository = memberPointHistoryRepository;
            _orderComplainImageRepository = orderComplainImageRepository;
            _sentryHub = sentryHub;
            _vnpayService = vnpayService;
            _socketService = socketService;
        }

        public async Task<JsonResultModel> CreateOrder(CreateOrderModel input, int CusID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    long productSumPrice = 0;
                    double totalWeight = 0;
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(CusID));
                    if (await _cartService.CheckCartChange(CusID))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_CART_UPDATED, SystemParam.MESSAGE_CART_UPDATED);
                    }
                    var listCart = await _cartRepository.GetAllAsync(x => input.CartID.Contains(x.ID) && x.IsActive.Equals(SystemParam.ACTIVE) && x.CustomerID.Equals(CusID), null, source => source.Include(x => x.ProductItem).ThenInclude(x => x.Product));
                    if (listCart.Count == 0)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_CART_EMPTY, SystemParam.MESSAGE_CART_EMPTY);
                    }
                    var receiveAddress = await _receiveAddressRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ReceiveAddressID) && x.IsActive.Equals(SystemParam.ACTIVE));
                    if (receiveAddress == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_RECEIVE_ADDRESS_NOT_FOUND, SystemParam.MESSAGE_RECEIVE_ADDRESS_NOT_FOUND);
                    }
                    foreach (var cart in listCart)
                    {
                        productSumPrice += cart.Quantity * cart.Price;
                        totalWeight += (cart.ProductItem.Product.NetWeight * cart.Quantity);
                        //var productQty = await _productStorageSerivce.GetProductQuantity(cart.ProductItem.ProductID);
                        //if (productQty.HasValue)
                        //{
                        //    if (productQty.GetValueOrDefault() == 0)
                        //    {
                        //        return JsonResponse.Error(SystemParam.ERROR_PRODUCT_CART_NOT_AVAILABLE, cart.ProductItem.Product.Name + " đã hết hàng");
                        //    }
                        //    else if (productQty < cart.Quantity)
                        //    {
                        //        return JsonResponse.Error(SystemParam.ERROR_PRODUCT_CART_EXCEED_QUANTITY, cart.ProductItem.Product.Name + " chỉ còn " + productQty + " sản phẩm");
                        //    }
                        //}
                    }
                    var order = new Order
                    {
                        Code = Util.GenerateCode("BH"),
                        CustomerID = CusID,
                        ProductSumPrice = productSumPrice,
                        TotalPrice = productSumPrice,
                        BuyerName = receiveAddress.Name,
                        BuyerPhone = receiveAddress.Phone,
                        BuyerAddress = receiveAddress.Address,
                        Note = input.Note,
                        ProvinceID = receiveAddress.ProvinceID,
                        DistrictID = receiveAddress.DistrictID,
                        WardID = receiveAddress.WardID,
                        Status = SystemParam.STATUS_ORDER_QUOTE,
                        ShipQuoteStatus = SystemParam.STATUS_QUOTE_NOT_QUOTED,
                        PaymentStatus = SystemParam.STATUS_PAYMENT_NOT_PAID,
                        TotalWeight = totalWeight
                    };
                    await _orderRepository.AddAsync(order);
                    var orderHistory = new OrderHistory
                    {
                        OrderID = order.ID,
                        Status = SystemParam.STATUS_ORDER_QUOTE,
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                    foreach (var cart in listCart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ProductID = cart.ProductItem.ProductID,
                            OrderID = order.ID,
                            Quantity = cart.Quantity,
                            Price = cart.Price,
                            SumPrice = cart.Quantity * cart.Price,
                            Weight = cart.ProductItem.Product.NetWeight
                        };
                        await _orderDetailRepository.AddAsync(orderDetail);
                        cart.IsActive = SystemParam.ACTIVE_FALSE;
                        await _cartRepository.UpdateAsync(cart);
                    }
                    await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_ORDER, string.Format(SystemParam.NOTIFICATION_TYPE_ORDER_NEW_STR, order.Code), order.ID, null, null);
                    scope.Complete();
                    return JsonResponse.Success(order.ID);
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListOrder(int Page, int Limit, int Status, int CusID)
        {
            try
            {
                var orderCounts = new OrderCountModel()
                {
                    Cancel = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_CANCEL, CusID),
                    Quote = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_QUOTE, CusID),
                    Complain = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_COMPLAIN, CusID),
                    Complete = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_COMPLETE, CusID),
                    Delivered = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_DELIVERED, CusID),
                    Delivering = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_DELIVERING, CusID),
                    Pending = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_PENDING, CusID),
                    Return = await _orderRepository.CountOrder(SystemParam.STATUS_ORDER_RETURN, CusID),
                };

                var model = new ListOrderModel
                {
                    OrderCount = orderCounts,
                    ListOrder = await _orderRepository.GetOrders(Page, Limit, Status, CusID)
                };

                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetOrderDetail(int ID)
        {
            try
            {
                var model = await _orderRepository.GetOrderDetail(ID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task<JsonResultModel> GetListOrder(int page, int limit, string searchKey, int? status, string startDate, string endDate)
        {
            try
            {
                var list = await _orderRepository.GetOrders(page, limit, searchKey, status, startDate, endDate);
                DataPagedListModel data = new DataPagedListModel
                {
                    Data = list,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = list.TotalItemCount
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> RequestCancelOrder(int ID)
        {
            try
            {
                var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                order.DeclineRequest = SystemParam.ACTIVE;
                await _orderRepository.UpdateAsync(order);
                await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_ORDER, string.Format(SystemParam.NOTIFICATION_TYPE_ORDER_REQUEST_CANCEL_STR, order.Code), order.ID, null, null);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> HandleRequestCancelOrder(int ID, int IsCancel)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));

                    if (IsCancel.Equals(SystemParam.ACTIVE))
                    {
                        order.Status = SystemParam.STATUS_ORDER_CANCEL;
                        var contentNoti = "Đơn hàng " + order.Code + " đã bị hủy";
                        var orderHistory = new OrderHistory
                        {
                            OrderID = order.ID,
                            Status = SystemParam.STATUS_ORDER_CANCEL
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                        var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(order.CustomerID));
                        cus.Point += order.UsePoint.GetValueOrDefault();
                        await _customerRepository.UpdateAsync(cus);
                        await _pushNotificationSerivce.PushNotification(cus, SystemParam.NOTIFICATION_TYPE_ORDER, contentNoti, null, order.ID);
                    }
                    else
                    {
                        var contentNoti = "Yêu cầu hủy đơn hàng " + order.Code + " của bạn đã bị từ chối";
                        var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(order.CustomerID));
                        await _pushNotificationSerivce.PushNotification(cus, SystemParam.NOTIFICATION_TYPE_ORDER, contentNoti, null, order.ID);
                    }
                    order.DeclineRequest = SystemParam.ACTIVE_FALSE;
                    await _orderRepository.UpdateAsync(order);
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

        public async Task<JsonResultModel> CancelOrder(int ID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                    if (order.Status.Equals(SystemParam.STATUS_ORDER_CANCEL))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_CANCEL, SystemParam.MESSAGE_ORDER_ALREADY_CANCEL);
                    }
                    else if (order.Status.Equals(SystemParam.STATUS_ORDER_COMPLETE))
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_COMPLETE, SystemParam.MESSAGE_ORDER_ALREADY_COMPLETE);
                    }
                    order.Status = SystemParam.STATUS_ORDER_CANCEL;
                    await _orderRepository.UpdateAsync(order);
                    var orderHistory = new OrderHistory
                    {
                        OrderID = order.ID,
                        Status = SystemParam.STATUS_ORDER_CANCEL
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(order.CustomerID));
                    cus.Point += order.UsePoint.GetValueOrDefault();
                    await _customerRepository.UpdateAsync(cus);
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

        public async Task<JsonResultModel> ChangeStatusOrder(ChangeStatusOrderModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(order.CustomerID));
                    string contentNoti = "";
                    if (input.Status != order.Status)
                    {
                        if (order.Status == SystemParam.STATUS_ORDER_CANCEL)
                        {
                            cus.Point += order.UsePoint.GetValueOrDefault();
                            await _customerRepository.UpdateAsync(cus);
                            return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_CANCEL, SystemParam.MESSAGE_ORDER_ALREADY_CANCEL);
                        }
                        if (order.Status == SystemParam.STATUS_ORDER_COMPLETE)
                        {
                            return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_COMPLETE, SystemParam.MESSAGE_ORDER_ALREADY_COMPLETE);
                        }
                        order.Status = input.Status;
                        await _orderRepository.UpdateAsync(order);
                        var orderHistory = new OrderHistory
                        {
                            OrderID = order.ID,
                            Status = input.Status
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                        switch (input.Status)
                        {
                            case SystemParam.STATUS_ORDER_CANCEL:
                                contentNoti = "Đơn hàng " + order.Code + " đã bị hủy";
                                break;
                            case SystemParam.STATUS_ORDER_DELIVERING:
                                contentNoti = "Đơn hàng " + order.Code + " đang được vận chuyển tới bạn";
                                break;
                            case SystemParam.STATUS_ORDER_DELIVERED:
                                contentNoti = "Đơn hàng " + order.Code + " đã được giao thành công";
                                break;
                        }
                    }
                    await _pushNotificationSerivce.PushNotification(cus, SystemParam.NOTIFICATION_TYPE_ORDER, contentNoti, null, order.ID);
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

        public async Task<JsonResultModel> CompleteOrder(int ID)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                    if (order.Status == SystemParam.STATUS_ORDER_CANCEL)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_CANCEL, SystemParam.MESSAGE_ORDER_ALREADY_CANCEL);
                    }
                    if (order.Status == SystemParam.STATUS_ORDER_COMPLETE)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_COMPLETE, SystemParam.MESSAGE_ORDER_ALREADY_COMPLETE);
                    }
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(order.CustomerID), null, source => source.Include(x => x.CustomerType));
                    order.Status = SystemParam.STATUS_ORDER_COMPLETE;
                    await _orderRepository.UpdateAsync(order);
                    var orderHistory = new OrderHistory
                    {
                        OrderID = order.ID,
                        Status = SystemParam.STATUS_ORDER_COMPLETE
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                    if (cus.CustomerTypeID != SystemParam.CUSTOMER_TYPE_NORMAL)
                    {
                        var addPoint = (long)(cus.CustomerType.BonusPointPerKg * order.TotalWeight);
                        cus.Point += addPoint;
                        await _customerRepository.UpdateAsync(cus);
                        var contentNoti = "Bạn vừa tích được " + addPoint + " điểm từ đơn hàng " + order.Code;
                        var mph = new MemberPointHistory
                        {
                            CustomerID = cus.ID,
                            Point = addPoint,
                            Balance = cus.Point,
                            OrderID = order.ID,
                            TypeAdd = SystemParam.TYPE_ADD_POINT,
                            Type = SystemParam.MEMBER_POINT_HISTORY_TYPE_COMPLETE_ORDER,
                            Description = contentNoti
                        };
                        await _memberPointHistoryRepository.AddAsync(mph);
                        await _pushNotificationSerivce.PushNotification(cus, SystemParam.NOTIFICATION_TYPE_BONUS_POINT, contentNoti, null, null);
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

        public async Task<JsonResultModel> ComplainOrder(ComplainOrderModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (input.ListImageUrl == null)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_COMPLAIN_NO_IMAGE, SystemParam.MESSAGE_ORDER_COMPLAIN_NO_IMAGE);
                    }
                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                    if (order.Status == SystemParam.STATUS_ORDER_CANCEL)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_CANCEL, SystemParam.MESSAGE_ORDER_ALREADY_CANCEL);
                    }
                    if (order.Status == SystemParam.STATUS_ORDER_COMPLETE)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_COMPLETE, SystemParam.MESSAGE_ORDER_ALREADY_COMPLETE);
                    }
                    order.Status = SystemParam.STATUS_ORDER_COMPLAIN;
                    order.NoteComplain = input.NoteComplain;
                    await _orderRepository.UpdateAsync(order);
                    var orderHistory = new OrderHistory
                    {
                        OrderID = order.ID,
                        Status = SystemParam.STATUS_ORDER_COMPLAIN
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                    foreach (var item in input.ListImageUrl)
                    {
                        var orderComplainImage = new OrderComplainImage
                        {
                            ImageUrl = item,
                            OrderID = order.ID
                        };
                        await _orderComplainImageRepository.AddAsync(orderComplainImage);
                    }
                    await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_ORDER, string.Format(SystemParam.NOTIFICATION_TYPE_ORDER_COMPLAIN_STR, order.Code), order.ID, null, null);
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

        public async Task<JsonResultModel> ShipQuoteOrder(ShipQuoteOrderModel input)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(input.ID));
                    var cus = await _customerRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(order.CustomerID));
                    order.ShipQuoteStatus = SystemParam.STATUS_QUOTE_QUOTED;
                    order.ShipFee = input.ShipFee;
                    order.TotalPrice += input.ShipFee;
                    await _orderRepository.UpdateAsync(order);
                    var contentNoti = "Đơn hàng " + order.Code + " đã được báo giá vận chuyển";
                    await _pushNotificationSerivce.PushNotification(cus, SystemParam.NOTIFICATION_TYPE_ORDER, contentNoti, null, order.ID);
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

        public async Task<JsonResultModel> PaymentOrder(Customer cus, int orderID, int paymentType, int Point)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    //lấy ra khách hàng và Order chưa thanh toán 

                    var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID == orderID && x.IsActive == SystemParam.ACTIVE);
                    if (order == null) return JsonResponse.Error(SystemParam.ERROR_NOT_FOUND_ORDER, SystemParam.MESSAGE_NOT_FOUND_ORDER);
                    if (order.Status == SystemParam.STATUS_ORDER_CANCEL) return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_CANCEL, SystemParam.MESSAGE_ORDER_ALREADY_CANCEL);
                    if (order.Status != SystemParam.STATUS_ORDER_QUOTE) return JsonResponse.Error(SystemParam.ERROR_ORDER_ALREADY_PAID, SystemParam.MESSAGE_ORDER_ALREADY_PAID);
                    // Trừ điểm của người dùng khi người ta sử dụng điểm
                    if (Point > 0)
                    {
                        // kiểm tra điểm của người đó 
                        if (cus.Point >= Point)
                        {
                            if (Point > order.TotalPrice)
                            {
                                Point = (int)order.TotalPrice;
                            }
                            order.TotalPrice -= Point;
                            order.UsePoint = order.UsePoint.GetValueOrDefault() + Point;
                            cus.Point -= Point;
                            await _customerRepository.UpdateAsync(cus);

                            // Thêm lịch sử điểm của người đó nếu có sử dụng điểm
                            MemberPointHistory mb = new MemberPointHistory
                            {
                                IsActive = SystemParam.ACTIVE,
                                CreatedDate = DateTime.Now,
                                Balance = cus.Point,
                                TypeAdd = SystemParam.TYPE_MINUS_POINT,
                                Point = Point,
                                Type = SystemParam.MEMBER_POINT_HISTORY_TYPE_USE_POINT,
                                CustomerID = cus.ID,
                                OrderID = orderID,
                                Description = SystemParam.MESSAGE_PAYMENT_USE_POINT
                            };
                            await _memberPointHistoryRepository.AddAsync(mb);
                        }
                        else
                        {
                            return JsonResponse.Error(SystemParam.ERROR, SystemParam.MESSAGE_NOT_ENOUGH_POINT);  // Trường hợp không đủ điểm để thanh toán 
                        }
                    }
                    if (order.TotalPrice < SystemParam.ORDER_VNPAY_MIN_PRICE && paymentType == SystemParam.PAYMENT_TYPE_VNPAY)
                    {
                        return JsonResponse.Error(SystemParam.ERROR_ORDER_VNPAY_LIMIT_MONEY, SystemParam.MESSAGE_ORDER_VNPAY_LIMIT_MONEY);
                    }
                    // Cập nhật đơn hàng
                    order.PaymentType = paymentType;
                    await _orderRepository.UpdateAsync(order);
                    if (paymentType == SystemParam.PAYMENT_TYPE_CASH || paymentType == SystemParam.PAYMENT_TYPE_TRANFER)
                    {
                        order.Status = SystemParam.STATUS_ORDER_PENDING;
                        await _orderRepository.UpdateAsync(order);
                        // tạo lịch sử đơn hàng : 
                        OrderHistory orderHistory = new OrderHistory()
                        {
                            OrderID = order.ID,
                            Status = SystemParam.STATUS_ORDER_PENDING
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                        await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_ORDER, string.Format(SystemParam.NOTIFICATION_TYPE_ORDER_CONFIRM_STR, order.Code), order.ID, null, null);
                    }
                    if (paymentType == SystemParam.PAYMENT_TYPE_VNPAY)
                    {
                        var urlVnpay = await _vnpayService.GetUrl(orderID);
                        scope.Complete();
                        return JsonResponse.Success(urlVnpay);
                    }
                    else
                    {
                        scope.Complete();
                        return JsonResponse.Success();
                    }

                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }


        public async Task<JsonResultModel> GetExportStorageForm(int ID)
        {
            try
            {
                var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID), null, source => source.Include(x => x.OrderDetails).ThenInclude(x => x.Product).Include(x => x.Customer));
                if (order == null) return JsonResponse.Error(SystemParam.ERROR_NOT_FOUND_ORDER, SystemParam.MESSAGE_NOT_FOUND_ORDER);
                var exportStorage = new CreateStorageExportOrderModel
                {
                    Customer = order.Customer.Name,
                    ProvinceID = order.ProvinceID,
                    ExportDate = DateTime.Now,
                    ReceiverName = order.BuyerName,
                    Reason = string.Format(SystemParam.REASON_EXPORT_STORAGE_ORDER, order.Code)
                };
                var listProductExport = new List<StorageExportProductDetailModel>();
                foreach (var item in order.OrderDetails)
                {
                    var productStorage = await _productStorageSerivce.GetAllAsync(x => x.ProductID.Equals(item.ProductID) && x.Quantity > 0, source => source.OrderBy(x => x.ExpiredDate),source => source.Include(x => x.Product));
                    var productStorageQty = productStorage.Sum(x => x.Quantity);
                    if (productStorageQty < item.Quantity)
                    {
                        var quantityMissing = item.Quantity - productStorageQty;
                        return JsonResponse.Error(SystemParam.ERROR_STORAGE_EXPORT_PRODUCT_ORDER_EXCEED_QUANTITY, string.Format(SystemParam.MESSAGE_STORAGE_EXPORT_PRODUCT_ORDER_EXCEED_QUANTITY, item.Product.Name, quantityMissing));
                    }
                    var totalQuantity = 0;
                    foreach (var subItem in productStorage)
                    {
                        // Sản phẩm trong kho lớn hơn số lượng hoặc bằng cần xuất
                        if (totalQuantity + subItem.Quantity >= item.Quantity)
                        {
                            var StorageExportProducts = new StorageExportProductDetailModel
                            {
                                StorageID = subItem.StorageID.GetValueOrDefault(),
                                ProductStorageID = subItem.ID,
                                Name = item.Product.Name,
                                Code = item.Product.Code,
                                Quantity = item.Quantity - totalQuantity,
                                Price = item.Price,
                                Supplier = subItem.Supplier,
                                ExpiredDate = subItem.ExpiredDate,
                                LotNo = subItem.LotNo,
                                ManufactureDate = subItem.ManufactureDate,
                                NetWeight = subItem.Product.NetWeight,
                                Unit = subItem.Product.Unit
                            };
                            listProductExport.Add(StorageExportProducts);
                            break;
                        }
                        else
                        {
                            var StorageExportProducts = new StorageExportProductDetailModel
                            {
                                StorageID = subItem.StorageID.GetValueOrDefault(),
                                ProductStorageID = subItem.ID,
                                Quantity = subItem.Quantity,
                                Name = item.Product.Name,
                                Code = item.Product.Code,
                                Price = item.Price,
                                Supplier = subItem.Supplier,
                                ExpiredDate = subItem.ExpiredDate,
                                LotNo = subItem.LotNo,
                                ManufactureDate = subItem.ManufactureDate,
                                NetWeight = subItem.Product.NetWeight,
                                Unit = subItem.Product.Unit
                            };
                            listProductExport.Add(StorageExportProducts);
                            totalQuantity += subItem.Quantity;
                        }
                    }
                }
                exportStorage.StorageExportProducts = listProductExport;
                return JsonResponse.Success(exportStorage);


            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task CompleteOrderProcedure(int CompleteOrderTime)
        {
            try
            {
                var order = await _orderRepository.GetAllAsync(x => x.Status.Equals(SystemParam.STATUS_ORDER_DELIVERED) && x.IsActive.Equals(SystemParam.ACTIVE));
                foreach (var item in order)
                {
                    var orderHistory = await _orderHistoryRepository.GetFirstOrDefaultAsync(x => x.OrderID.Equals(item.ID) && x.Status.Equals(SystemParam.STATUS_ORDER_DELIVERED));
                    // Đơn hàng đã giao quá 3 ngày sẽ tự động hoàn thành
                    if(DateTime.Now > orderHistory.CreatedDate.AddDays(CompleteOrderTime)){
                        await CompleteOrder(item.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
            }
        }
    }
}
