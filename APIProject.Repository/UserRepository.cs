using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using APIProject.Service.Utils;
using PagedList;
using PagedList.Core;
using System.Threading;
using APIProject.Common.Models.Users;

namespace APIProject.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IPagedList<UserModel>> GetUsers(int page, int limit,string SearchKey,int? role, int? status, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from u in DbContext.Users
                                 join r in DbContext.Roles on u.RoleID equals r.ID
                                 where u.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!string.IsNullOrEmpty(SearchKey) ? (u.Username.Contains(SearchKey) || u.Phone.Contains(SearchKey)) : true)
                                 && (fd.HasValue ? u.CreatedDate >= fd : true)
                                 && (td.HasValue ? u.CreatedDate <= td : true)
                                 && (role.HasValue ? u.RoleID.Equals(role) :true)
                                 && (status.HasValue ? u.Status.Equals(status) : true)
                                 orderby u.CreatedDate descending
                                 select new UserModel
                                 {
                                    ID = u.ID,
                                    Email = u.Email,
                                    Phone = u.Phone,
                                    RoleName = r.Name,
                                    Username = u.Username,
                                    Status = u.Status
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
