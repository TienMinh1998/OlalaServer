using APIProject.Service.Models;
using APIProject.Domain.Models;
using APIProject.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Models.Authentication;
using APIProject.Service.Models.Customer;
using APIProject.Common.Models.Password;

namespace APIProject.Service.Interface
{
    public interface ICustomerService : IServices<Customer>
    {
        Task<JsonResultModel> Register(RegisterModel model, string secretKey, int timeout);
        Task<JsonResultModel> Authenticate(LoginModel model, string secretKey, int timeout);
        Task<JsonResultModel> GetCustomers(int page, int limit, int? customerType, int? status, string searchKey, string startDate, string endDate);
        int? GetCustomerType(Customer model);
        Task<JsonResultModel> GetUserInfo(int ID);
        Task<JsonResultModel> ConfirmRole(int[] IDs);
        Task<JsonResultModel> ChangeStatus(int customerID);
        Task<JsonResultModel> ChangeAvatar(Customer customer, string ImageUrl);
        Task<JsonResultModel> UpdateUserInfo(Customer customer, ChangeCustomerInfoModel input);
        Task<JsonResultModel> GetMemberPointHistory(int Page, int Limit, int Type, int CusID, string StartDate, string EndDate);
        Task<JsonResultModel> ForgotPassword(string phone);
        Task<JsonResultModel> ConfirmOTP(ConfirmOTPModel model);
        Task<JsonResultModel> ChangePasswordOTP(ChangePasswordOTPModel model);
        Task<JsonResultModel> ChangePassword(Customer cus ,string Password);

    }
}
