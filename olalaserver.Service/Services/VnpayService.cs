using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Library;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sentry;
using APIProject.Service.Models.Vnpay;
using Newtonsoft.Json;
using APIProject.Domain.Models;

namespace APIProject.Service.Services
{
    public class VnpayService : IVnpayService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly VnPayLibrary vnpay;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IHub _sentryHub;
        private readonly ISocketService _socketService;
        public VnpayService(IHttpContextAccessor httpContextAccessor, VnPayLibrary vnpay, IOrderRepository orderRepository, IHub sentryHub, IOrderHistoryRepository orderHistoryRepository, ISocketService socketService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.vnpay = vnpay;
            _orderRepository = orderRepository;
            _sentryHub = sentryHub;
            _orderHistoryRepository = orderHistoryRepository;
            _socketService = socketService;
        }
        public async Task<string> GetUrl(int orderID)
        {
            //Get Config Info
            //Get payment input
            //Build URL for VNPAY
            var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(orderID));
            var Content = "Thanh toan don hang " + order.Code + " .So tien " + Util.ConvertCurrency(order.TotalPrice) + " VND";
            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", SystemParam.vnp_TmnCode);
            string locale = "vn";//"en"
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.ID.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", Content);
            vnpay.AddRequestData("vnp_OrderType", "insurance");
            vnpay.AddRequestData("vnp_Amount", (order.TotalPrice * 100).ToString());
            vnpay.AddRequestData("vnp_ReturnUrl", SystemParam.vnp_Return_url);
            vnpay.AddRequestData("vnp_IpAddr", GetIpAddress());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string paymentUrl = vnpay.CreateRequestUrl(SystemParam.vnp_Url, SystemParam.vnp_HashSecret);
            _sentryHub.CaptureMessage(paymentUrl);
            return paymentUrl;
        }

        public string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
                    ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }


        public async Task<VnpViewModel> GetVnpReturn(VnpOutputModel vnp)
        {
            string json = JsonConvert.SerializeObject(vnp);
            _sentryHub.CaptureMessage("url_ipn" + json);
            VnpViewModel vnpOutput = new VnpViewModel();
            string Transaction_Success = SystemParam.TRANSACTION_SUCCESS;
            string Transaction_False = SystemParam.TRANSACTION_FAIL;
            try
            {
                var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(vnp.vnp_TxnRef));
                long money;
                try
                {
                    money = Int32.Parse(vnp.vnp_Amount) / 100;
                    if (money != order.TotalPrice)
                    {
                        vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", money), order.CreatedDate.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed);
                        return vnpOutput;
                    }
                }
                catch (Exception ex)
                {
                    string jsonEx = JsonConvert.SerializeObject(ex);
                    _sentryHub.CaptureException(ex);
                    vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", order.TotalPrice), order.CreatedDate.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed);
                    return vnpOutput;
                }

                if (vnp.vnp_ResponseCode == SystemParam.vnp_CodeSucces)
                {
                    if (order != null)
                    {
                        if (order.Status == SystemParam.STATUS_ORDER_CANCEL)
                        {
                            vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", order.TotalPrice), order.CreatedDate.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed + order.ID);
                        }
                        else
                        {
                            vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", order.TotalPrice), order.CreatedDate.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_Success, SystemParam.customer_success + order.ID);
                        }
                    }
                    else
                    {
                        vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", 0), DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed);
                    }
                }
                else
                {
                    if (order != null)
                    {
                        vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", order.TotalPrice), order.CreatedDate.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed + order.ID);

                    }
                    else
                        vnpOutput.getVnpModel(vnp.vnp_TxnRef, string.Format("{0:#,0}", 0), DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed);
                }
            }
            catch (Exception ex)
            {
                string jsonEx = JsonConvert.SerializeObject(ex);
            
                _sentryHub.CaptureException(ex);
                vnpOutput.getVnpModel(vnp.vnp_TxnRef, "", DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), Transaction_False, SystemParam.customer_failed);
            }
            return vnpOutput;
        }
        public async Task<VNPayOutputModel> GetVnpIpn(VnpOutputModel vnp)
        {
            VNPayOutputModel output = new VNPayOutputModel();
            try
            {
                string json = JsonConvert.SerializeObject(vnp);
                _sentryHub.CaptureMessage("url_ipn" + json);
                var order = await _orderRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(vnp.vnp_TxnRef));
                if (order == null)
                {
                    output = output.GetPayOutputModel("Order not found", "01");
                    return output;
                }
                if (order != null)
                {
                    int money = 0;
                    try
                    {
                        money = int.Parse(vnp.vnp_Amount) / 100;
                        if (money != order.TotalPrice)
                        {
                            output = output.GetPayOutputModel("Invalid amount", "04");
                            return output;
                        }
                    }
                    catch
                    {
                        output = output.GetPayOutputModel("Invalid amount", "04");
                        return output;
                    }
                    //bool checkSignature = vnpay.ValidateSignature(vnp.vnp_SecureHash, SystemParam.vnp_HashSecret, vnp);
                    bool checkSignature = true;
                    if (checkSignature)
                    {
                        try
                        {
                            if (vnp.vnp_ResponseCode == SystemParam.vnp_CodeSucces)
                            {
                                // Chuyển trạng thái của đơn hàng
                                order.Status = SystemParam.STATUS_ORDER_PENDING;
                                order.PaymentStatus = SystemParam.STATUS_PAYMENT_PAID;
                                await _orderRepository.UpdateAsync(order);
                                // lưu lại lịch sử 
                                OrderHistory orderHistory = new OrderHistory()
                                {
                                    OrderID = order.ID,
                                    Status = SystemParam.STATUS_ORDER_PENDING
                                };
                                await _orderHistoryRepository.AddAsync(orderHistory);
                                await _socketService.PushSocket(SystemParam.NOTIFICATION_TYPE_ORDER, string.Format(SystemParam.NOTIFICATION_TYPE_ORDER_CONFIRM_STR, order.Code), order.ID, null, null);
                            }
                            else
                            {
                                output = output.GetPayOutputModel("Invalid amount", "04");
                                return output;
                            }
                        }
                        catch (Exception ex)
                        {
                            output = output.GetPayOutputModel("Unknow error", "99");
                            //oneSignalBus.SaveLog("Exception", ex.ToString());
                            _sentryHub.CaptureException(ex);
                        }
                    }
                    else
                    {
                        output = output.GetPayOutputModel("Invalid signature", "97");
                    }
                }
                else output = output.GetPayOutputModel("Order not found", "01");
            }
            catch (Exception ex)
            {
                output = output.GetPayOutputModel("Unknow error", "99");
                //oneSignalBus.SaveLog("Exception", ex.ToString());
                _sentryHub.CaptureException(ex);
            }
            //oneSignalBus.SaveLog(output.RspCode, output.Message);
            _sentryHub.CaptureMessage(output.RspCode + output.Message);

            return output;
        }
    }
}
