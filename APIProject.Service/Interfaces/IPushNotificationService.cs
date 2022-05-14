using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using APIProject.Service.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IPushNotificationService
    {
        void PushOneSignal(string value);
        string CreateOneSignalInput(object obj, List<string> deviceID, string contents);
        Task PushNotification(Customer cus, int type, string content, int? NewsID, int? OrderID);
        Task PushNotification(IList<Customer> listCus, int type, string content, int? NewsID, int? OrderID,int? ProductID);
        Task<JsonResultModel> SendNotification(Customer Cus, NotificationModel input);
    }
}
