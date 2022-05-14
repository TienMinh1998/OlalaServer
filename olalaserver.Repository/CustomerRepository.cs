


/*------------------------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 15/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : hàm đếm số điện thoại, hàm đếm Email
 * -----------------------------------------------*/

using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using APIProject.Service.Utils;
using APIProject.Common.Models.Customer;
using APIProject.Common.Models.Product;

namespace APIProject.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<int> CountCustomer()
        {
            try
            {
                return await DbContext.Customers.Where(x => x.IsActive.Equals(SystemParam.ACTIVE)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountEmailOfCustomer(string Email)
        {
            return await DbContext.Customers.Where(x => x.Email.Equals(Email)).CountAsync();
        }
        public async Task<int> CountPhoneOfCustomer(string Phone)
        {
            return await DbContext.Customers.Where(x => x.Phone.Equals(Phone)).CountAsync();
        }

        public async Task<IPagedList<CustomerWebModel>> GetCustomers(int page, int limit, int? cusType, int? status, string searchKey, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from cus in DbContext.Customers
                                 where cus.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!string.IsNullOrEmpty(searchKey) ? (cus.Name.Contains(searchKey) || cus.Phone.Contains(searchKey)) : true)
                                 && (fd.HasValue ? cus.CreatedDate >= fd : true)
                                 && (td.HasValue ? cus.CreatedDate <= td : true)
                                 && (cusType.HasValue ? cus.CustomerTypeID.Equals(cusType) : true)
                                 && (status.HasValue ? cus.Status.Equals(status) : true)
                                 orderby cus.CreatedDate descending
                                 select new CustomerWebModel
                                 {
                                     ID = cus.ID,
                                     Name = cus.Name,
                                     Phone = cus.Phone,
                                     Type = cus.CustomerType.Name,
                                     Status = cus.Status,
                                     Email = cus.Email,
                                     IsConfirm = cus.IsConfirmRole,
                                     CreateDate = cus.CreatedDate
                                 }).AsQueryable().ToPagedList(page, limit);
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
