using APIProject.Common.Models.Order;
using APIProject.Domain.Models;
using APIProject.Service.Models;
using APIProject.Service.Models.Order;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IOrderService
    {
        Task<JsonResultModel> CreateOrder(CreateOrderModel input, int CusID);
        Task<JsonResultModel> RequestCancelOrder(int ID);
        Task<JsonResultModel> CancelOrder(int ID);
        Task<JsonResultModel> GetExportStorageForm(int ID);
        Task<JsonResultModel> ChangeStatusOrder(ChangeStatusOrderModel input);
        Task<JsonResultModel> CompleteOrder(int ID);
        Task CompleteOrderProcedure(int CompleteOrderTime);
        Task<JsonResultModel> HandleRequestCancelOrder(int ID,int IsCancel);
        Task<JsonResultModel> ShipQuoteOrder(ShipQuoteOrderModel input);
        Task<JsonResultModel> ComplainOrder(ComplainOrderModel input);
        Task<JsonResultModel> GetListOrder(int Page, int Limit, int Status, int CusID);
        Task<JsonResultModel> GetOrderDetail(int ID);
        Task<JsonResultModel> GetListOrder(int page, int limit, string searchKey, int? status, string startDate, string endDate);
        Task<JsonResultModel> PaymentOrder(Customer cus, int orderID, int paymentType, int Point);
    }
}
