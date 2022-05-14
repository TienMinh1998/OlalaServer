using APIProject.Domain.Models;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Notification;
using APIProject.Service.Utils;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly IHub _sentryHub;

        public PushNotificationService(INotificationService notificationService, IHub sentryHub)
        {
            _notificationService = notificationService;
            _sentryHub = sentryHub;
        }

        public async Task PushNotification(Customer cus, int type, string content, int? NewsID, int? OrderID)
        {
            await _notificationService.CreateNotification(cus, content, type, OrderID, NewsID);
            NotifyDataModel notifyData = new NotifyDataModel()
            {
                id = NewsID.HasValue ? NewsID.Value : OrderID.HasValue ? OrderID.Value : 0,
                type = type
            };
            List<string> listDevice = new List<string>();
            if(cus.DeviceID != null && cus.DeviceID.Length > 10)
            {
                listDevice.Add(cus.DeviceID);
            }
            string value = CreateOneSignalInput(notifyData, listDevice,content);
            PushOneSignal(value);
        }

        public async Task PushNotification(IList<Customer> listCus, int type, string content, int? NewsID, int? OrderID,int? ProductID)
        {
            await _notificationService.CreateNotification(listCus, content, type, OrderID, NewsID,ProductID);
            NotifyDataModel notifyData = new NotifyDataModel()
            {
                id = NewsID.HasValue ? NewsID.Value : OrderID.HasValue ? OrderID.Value : ProductID.HasValue ?  ProductID.Value : 0,
                type = type
            };
            List<string> listDevice = new List<string>();
            foreach(var item in listCus)
            {
                if (item.DeviceID != null && item.DeviceID.Length > 10)
                {
                    listDevice.Add(item.DeviceID);
                }
            }
            string value = CreateOneSignalInput(notifyData, listDevice, content);
            PushOneSignal(value);
        }
        public string CreateOneSignalInput(object obj, List<string> deviceID, string contents)
        {
            var appid = SystemParam.APP_ID;
            var channelid = SystemParam.ANDROID_CHANNEL_ID;
            OneSignalInput input = new OneSignalInput();
            TextInput header = new TextInput();
            header.en = contents.Length > 0 ? SystemParam.NOTI_TITLE : "";
            TextInput content = new TextInput();
            content.en = contents.Length > 0 ? contents : "";
            input.app_id = appid;
            input.data = obj;
            input.headings = header;
            input.contents = content;
            input.android_channel_id = channelid;
            input.include_player_ids = deviceID;
            return JsonConvert.SerializeObject(input);
        }


        public void PushOneSignal(string value)
        {
            string url = SystemParam.URL_ONESIGNAL;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12;
            var req = HttpWebRequest.Create(string.Format(url));

            req.Headers["Authorization"] = SystemParam.Authorization;
            req.Headers["https"] = SystemParam.URL_BASE_https;
            var byteData = Encoding.UTF8.GetBytes(value);
            req.ContentType = "application/json";
            req.Method = "POST";
            try
            {
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }
                var response = (HttpWebResponse)req.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
            }
        }
        public async Task<JsonResultModel> SendNotification(Customer Cus, NotificationModel input)
        {
            try
            {
                await PushNotification(Cus, input.Type, input.Content, input.NewsID, input.OrderID);
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
