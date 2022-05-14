using APIProject.Common.Models.Order;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Utils;
using Microsoft.EntityFrameworkCore;
using APIProject.Common.Models.Statistic;

namespace APIProject.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountOrder()
        {
            try
            {
                return await DbContext.Orders.Where(x => x.IsActive.Equals(SystemParam.ACTIVE)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountOrder(int Status, int CusID)
        {
            try
            {
                return await DbContext.Orders.Where(x => x.CustomerID.Equals(CusID) && x.Status.Equals(Status)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderDetailModel> GetOrderDetail(int ID)
        {
            try
            {
                var query = await (from o in DbContext.Orders
                                   where o.ID.Equals(ID)
                                   select new OrderDetailModel
                                   {
                                       ID = o.ID,
                                       Code = o.Code,
                                       ProductSumPrice = o.ProductSumPrice,
                                       ShipQuoteStatus = o.ShipQuoteStatus,
                                       Status = o.Status,
                                       CreatedDate = o.CreatedDate,
                                       PaymentType = o.PaymentType,
                                       Address = o.BuyerAddress,
                                       Province = o.Province.Name,
                                       District = o.District.Name,
                                       Ward = o.Ward.Name,
                                       DeclineNote = o.DeclineNote,
                                       DeclineRequest = o.DeclineRequest,
                                       Note = o.Note,
                                       NoteComplain = o.NoteComplain,
                                       ShipFee = o.ShipFee,
                                       UsePoint = o.UsePoint,
                                       TotalPrice = o.TotalPrice,
                                       BuyerName = o.BuyerName,
                                       BuyerPhone = o.BuyerPhone,
                                       ListComplainImage = DbContext.OrderComplainImages.Where(oci => oci.OrderID.Equals(o.ID)).Select(oci => oci.ImageUrl).ToList(),
                                       ListHistoryOrder = (from oh in DbContext.OrderHistories
                                                           where oh.OrderID.Equals(o.ID)
                                                           select new HistoryOrderModel
                                                           {
                                                               CreateDate = oh.CreatedDate,
                                                               Status = oh.Status,
                                                           }).ToList(),
                                       ListProduct = (from od in DbContext.OrderDetails
                                                      join p in DbContext.Products on od.ProductID equals p.ID
                                                      where od.OrderID.Equals(o.ID)
                                                      select new ProductOrderModel
                                                      {
                                                          ID = od.ProductID,
                                                          Price = od.Price,
                                                          Quantity = od.Quantity,
                                                          ProductCode = p.Code,
                                                          Name = p.Name,
                                                          Unit = p.Unit,
                                                          ImageUrl = p.ProductImages.Select(x => x.ImageUrl).FirstOrDefault()
                                                      }).ToList()
                                   }).FirstOrDefaultAsync();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IPagedList<OrderModel>> GetOrders(int Page, int Limit, int Status, int CusID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var query = (from o in DbContext.Orders
                                 where o.IsActive.Equals(SystemParam.ACTIVE) && o.Status.Equals(Status) && o.CustomerID.Equals(CusID)
                                 orderby o.CreatedDate descending
                                 select new OrderModel
                                 {
                                     ID = o.ID,
                                     Code = o.Code,
                                     TotalPrice = o.TotalPrice,
                                     ShipQuoteStatus = o.ShipQuoteStatus,
                                     Status = o.Status,
                                     CreatedDate = o.CreatedDate,
                                     DeclineRequest = o.DeclineRequest,
                                     ListProduct = (from od in DbContext.OrderDetails
                                                    join p in DbContext.Products on od.ProductID equals p.ID
                                                    where od.OrderID.Equals(o.ID)
                                                    select new ProductOrderModel
                                                    {
                                                        ID = od.ProductID,
                                                        ImageUrl = DbContext.ProductImages.Where(x => x.ProductID.Equals(p.ID)).Select(x => x.ImageUrl).FirstOrDefault(),
                                                        Name = p.Name,
                                                        ProductCode = p.Code,
                                                        Unit = p.Unit,
                                                        Price = od.Price,
                                                        Quantity = od.Quantity,
                                                    }).ToList()
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return query;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IPagedList<OrderWebModel>> GetOrders(int page, int limit, string searchKey, int? status, string startDate, string endDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(startDate);
                    var td = Util.ConvertToDate(endDate);
                    var model = (from o in DbContext.Orders
                                 where o.IsActive.Equals(SystemParam.ACTIVE)
                                 && (fd.HasValue ? o.CreatedDate >= fd : true)
                                 && (td.HasValue ? o.CreatedDate <= td : true)
                                 && (!string.IsNullOrEmpty(searchKey) ? (o.Code.Contains(searchKey) || o.Customer.Phone.Contains(searchKey) || o.Customer.Name.Contains(searchKey)) : true)
                                 && (status.HasValue ? o.Status.Equals(status) : true)
                                 orderby o.CreatedDate descending
                                 select new OrderWebModel
                                 {
                                     ID = o.ID,
                                     Code = o.Code,
                                     CustomerName = o.Customer.Name,
                                     Phone = o.Customer.Phone,
                                     TotalPrice = o.TotalPrice,
                                     PaymentType = o.PaymentType,
                                     Status = o.Status,
                                     DeclineRequest = o.DeclineRequest,
                                     ShipQuoteStatus = o.ShipQuoteStatus,
                                     CreatedDate = o.CreatedDate
                                 }).AsQueryable().ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IPagedList<StatisticModel>> GetListSales(int page, int limit, string productCode, string productName, string startDate, string endDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(startDate);
                    var td = Util.ConvertToDate(endDate);
                    var model = (from o in DbContext.Orders
                                 where o.IsActive.Equals(SystemParam.ACTIVE)
                                 && (fd.HasValue ? o.CreatedDate >= fd : true)
                                 && (td.HasValue ? o.CreatedDate <= td : true)
                                 && (!string.IsNullOrEmpty(productCode) ? (o.Code.Contains(productCode)) : true)
                                 && (!string.IsNullOrEmpty(productName) ? (o.Customer.Name.Contains(productName)) : true)
                                 && (o.Status.Equals(SystemParam.STATUS_ORDER_COMPLETE))
                                 orderby o.CreatedDate descending
                                 select new StatisticModel
                                 {
                                     ID = o.ID,
                                     Code = o.Code,
                                     CustomerName = o.Customer.Name,
                                     SumPrice = o.TotalPrice,
                                     CreateDate = o.CreatedDate
                                 }).AsQueryable().ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> GetTotalSale(string productCode, string customerName, string startDate, string endDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(startDate);
                    var td = Util.ConvertToDate(endDate);
                    var model = (from o in DbContext.Orders
                                 where o.IsActive.Equals(SystemParam.ACTIVE)
                                 && (fd.HasValue ? o.CreatedDate >= fd : true)
                                 && (td.HasValue ? o.CreatedDate <= td : true)
                                 && (!string.IsNullOrEmpty(productCode) ? (o.Code.Contains(productCode)) : true)
                                 && (!string.IsNullOrEmpty(customerName) ? (o.Customer.Name.Contains(customerName)) : true)
                                 && (o.Status.Equals(SystemParam.STATUS_ORDER_COMPLETE))
                                 select o.TotalPrice).Sum();
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> SumPrice(string productCode, string productName, string startDate, string endDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(startDate);
                    var td = Util.ConvertToDate(endDate);
                    long totalPrice = 0;
                    totalPrice = (from o in DbContext.Orders
                                  where o.IsActive.Equals(SystemParam.ACTIVE)
                                  && (fd.HasValue ? o.CreatedDate >= fd : true)
                                  && (td.HasValue ? o.CreatedDate <= td : true)
                                  && (!string.IsNullOrEmpty(productCode) ? (o.Code.Contains(productCode)) : true)
                                  && (!string.IsNullOrEmpty(productName) ? (o.Customer.Name.Contains(productName)) : true)
                                  && (o.Status.Equals(SystemParam.STATUS_ORDER_COMPLETE))
                                  select o).Sum(x => x.TotalPrice);
                    return totalPrice;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
