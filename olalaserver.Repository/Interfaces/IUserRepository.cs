
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 13/12/2021
 * Edit     : NOT YET
 * Content  : User Repository 
 * ----------------------------------*/


using PagedList;
using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PagedList.Core;
using System.Threading;
using APIProject.Common.Models.Users;

namespace APIProject.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
     Task<IPagedList<UserModel>> GetUsers(int page, int limit, string SearchKey, int? role, int? status, string fromDate, string toDate);
    }


}
