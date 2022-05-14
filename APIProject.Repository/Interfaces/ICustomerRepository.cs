using APIProject.Common.Models.Customer;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer> 
    {
        Task<int> CountEmailOfCustomer(string Email);
        Task<int> CountPhoneOfCustomer(string Phone);
        Task<int> CountCustomer();
        Task<IPagedList<CustomerWebModel>>GetCustomers(int page, int limit, int? cusType, int? status, string searchKey, string fromDate, string toDate);
    }
}
