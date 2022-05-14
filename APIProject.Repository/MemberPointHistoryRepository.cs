using APIProject.Common.Models.MemberPointHistory;
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

namespace APIProject.Repository
{
    public class MemberPointHistoryRepository : BaseRepository<MemberPointHistory>, IMemberPointHistoryRepository
    {
        public MemberPointHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IPagedList<MemberPointHistoryModel>> GetMemberPointHistories(int Page, int Limit, int Type, int CusID, string StartDate, string EndDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fromDate = Util.ConvertFromDate(StartDate);
                    var toDate = Util.ConvertToDate(EndDate);
                    var model = (from mph in DbContext.MemberPointHistories
                                 where mph.CustomerID.Equals(CusID)
                                 && (Type != SystemParam.TYPE_MPH_ALL ? mph.Type.Equals(Type) : true)
                                 && (fromDate.HasValue ? mph.CreatedDate >= fromDate : true)
                                  && (toDate.HasValue ? mph.CreatedDate <= toDate : true)
                                 orderby mph.ID descending
                                 select new 
                                 {
                                     ID = mph.ID,
                                     TypeAdd = mph.TypeAdd,
                                     Type = mph.Type,
                                     TotalWeight = mph.Type.Equals(SystemParam.TYPE_MPH_EARN_POINT) ? (mph.OrderID.HasValue ? mph.Order.TotalWeight : (double?)null) : (double?)null,
                                     Balance = mph.Balance,
                                     Point = mph.Point,
                                     OrderCode = mph.Order.Code,
                                     OrderID = mph.OrderID,
                                     CreatedDate = mph.CreatedDate
                                 }).AsEnumerable().Select(x => new MemberPointHistoryModel
                                 {
                                     ID = x.ID,
                                     TypeAdd = x.TypeAdd,
                                     Type = x.Type,
                                     TotalWeight = x.TotalWeight,
                                     Balance = x.Balance,
                                     Point = x.Point,
                                     OrderCode = x.OrderCode,
                                     CreatedDate = x.CreatedDate,
                                     ImageUrl = (from pi in DbContext.ProductImages
                                                 join p in DbContext.Products on pi.ProductID equals p.ID
                                                 join od in DbContext.OrderDetails on p.ID equals od.ProductID
                                                 join o in DbContext.Orders on od.OrderID equals o.ID
                                                 where o.ID.Equals(x.OrderID)
                                                 select pi.ImageUrl).FirstOrDefault()
                                 }).AsQueryable().ToPagedList(Page, Limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
