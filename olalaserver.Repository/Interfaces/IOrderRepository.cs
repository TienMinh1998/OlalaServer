using APIProject.Common.Models.Order;
using APIProject.Common.Models.Statistic;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IPagedList<OrderWebModel>> GetOrders(int page, int limit, string searchKey, int? status, string startDate, string endDate);

        Task<IPagedList<OrderModel>> GetOrders(int Page, int Limit, int Status, int CusID);
        Task<int> CountOrder(int Status, int CusID);
        Task<int> CountOrder();
        Task<OrderDetailModel> GetOrderDetail(int ID);
        Task<IPagedList<StatisticModel>> GetListSales(int page, int limit, string productCode, string customerName, string startDate, string endDate);
        Task<long> GetTotalSale(string productCode, string customerName, string startDate, string endDate);
        Task<long> SumPrice(string productCode, string productName, string startDate, string endDate);
    }
}
