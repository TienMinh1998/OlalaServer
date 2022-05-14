using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Utils;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class SocketService : ISocketService
    {
        private readonly IHub _sentryHub;
        private readonly IHttpRequestService _httpRequestService;
        private readonly INotificationRepository _notificationRepository;

        public SocketService(IHub sentryHub, IHttpRequestService httpRequestService, INotificationRepository notificationRepository)
        {
            _sentryHub = sentryHub;
            _httpRequestService = httpRequestService;
            _notificationRepository = notificationRepository;
        }

        public async Task PushSocket(int type, string content, int? orderID, string cusPhone, string productCode)
        {
            try
            {
                var noti = new Notification
                {
                    Content = content,
                    OrderID = orderID,
                    IsAdmin = SystemParam.ACTIVE,
                    IsActive = SystemParam.ACTIVE,
                    CreatedDate = DateTime.Now,
                    Viewed = SystemParam.ACTIVE_FALSE,
                    CustomerPhone = cusPhone,
                    ProductCode = productCode,
                    Type = type,
                };
                await _notificationRepository.AddAsync(noti);
                var url = "";
                if (type.Equals(SystemParam.NOTIFICATION_TYPE_ORDER))
                {
                    url = SystemParam.URL_WEB_SOCKET + "?noti_id=" + noti.ID + "&content=" + content + "&order_id=" + orderID + "&type=" + type;
                }else if (type.Equals(SystemParam.NOTIFICATION_TYPE_REQUEST_ROLE))
                {
                    url = SystemParam.URL_WEB_SOCKET + "?noti_id=" + noti.ID + "&content=" + content + "&customer_phone=" + cusPhone + "&type=" + type;
                }else if (type.Equals(SystemParam.NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING))
                {
                    url = SystemParam.URL_WEB_SOCKET + "?noti_id=" + noti.ID + "&content=" + content + "&product_code=" + productCode + "&type=" + type;
                }
                _httpRequestService.CreateGetRequest(url);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
            }
        }
    }
}
