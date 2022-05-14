using APIProject.Service.Interface;
using APIProject.Service.Models;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using PagedList.Core;
using APIProject.Service.Models.Authentication;
using APIProject.Common.Models.Users;

namespace APIProject.Service.Interfaces
{
    public interface IUserService : IServices<User>
    {
        Task<JsonResultModel> Authenticate(LoginModel model, string secretKey, int timeout);
        Task<JsonResultModel> CreateUser(CreateUserModel usermodel);
        Task<JsonResultModel> UpdateUser(UpdateUserModel model);
        Task<JsonResultModel> ChangePassword(int id, string oldPass,string newPass);
        Task<JsonResultModel> DeleteUser(int ID);
        Task<JsonResultModel> GetUserDetail(int ID);
        Task<JsonResultModel> GetListUser(int page, int limit, string SearchKey, int? role, int? status, string fromDate, string toDate);
    }
}
