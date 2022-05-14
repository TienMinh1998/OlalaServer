using APIProject.Common.Models.News;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
   public interface INewsRepository : IRepository<News>
    {
        
      
        Task<IPagedList<NewsWebModel>> GetListNews(int page, int limit, string TitleBanner,int? Type, int? TypeNews, int? status, string fromDate, string toDate);
    }
}
