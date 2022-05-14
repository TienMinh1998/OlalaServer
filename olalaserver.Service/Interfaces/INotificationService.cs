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
    public interface INotificationService : IServices<Notification>
    {
        Task CreateNotification(Customer Cus, string Content, int Type, int? OrderID, int? NewsID);
        Task CreateNotification(IList<Customer> ListCus, string Content, int Type, int? OrderID, int? NewsID,int? ProductID);
        Task CreateNotificationAdmin(string Content, int Type, int? OrderID, int? NewsID);
        Task<JsonResultModel> GetListNotification(int Page, int Limit, int CusID);
        Task<JsonResultModel> GetListNotificationAdmin(int Page, int Limit,int RoleID);
        Task<JsonResultModel> ReadNotification(int ID);
        Task<JsonResultModel> ReadAllNotification(int CusID);
        Task<JsonResultModel> CountNotificationNotRead(int CusID);
        Task<JsonResultModel> ReadAllNotificationAdmin(int RoleID);
        Task<JsonResultModel> CountNotificationNotReadAdmin(int RoleID);
        List<int> ConvertPermissionsToNoificationTypes(List<int> listPermission);


    }
}
