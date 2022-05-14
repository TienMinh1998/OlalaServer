using APIProject.Common.Models.MemberPointHistory;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IMemberPointHistoryRepository : IRepository<MemberPointHistory>
    {
        Task<IPagedList<MemberPointHistoryModel>> GetMemberPointHistories(int Page, int Limit, int Type, int CusID, string StartDate, string EndDate);
    }
}
