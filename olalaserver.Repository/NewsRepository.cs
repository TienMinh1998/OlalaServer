using APIProject.Common.Models.News;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Service.Utils;
using PagedList.Core;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using APIProject.Repository.Interfaces;

namespace APIProject.Repository
{
 public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IPagedList<NewsWebModel>> GetListNews(int page, int limit, string TitleBanner, int? Type, int? TypeNews, int? status, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from n in DbContext.News
                                 where n.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!string.IsNullOrEmpty(TitleBanner) ? n.Title.Contains(TitleBanner) : true)
                                 && (fd.HasValue ? n.CreatedDate >= fd : true)
                                 && (td.HasValue ? n.CreatedDate <= td : true)
                                 && (status.HasValue ? n.Status.Equals(status) : true)
                                 && (TypeNews.HasValue ? n.TypeNews.Equals(TypeNews) : true)
                                  && (Type.HasValue ? n.Type.Equals(Type) : true)
                                 orderby n.CreatedDate descending
                                 select new NewsWebModel
                                 {
                                    ID = n.ID,
                                    CreatedDate = n.CreatedDate,
                                    Status = n.Status,
                                    Title = n.Title,
                                    Type = n.Type,
                                    TypeNews = n.TypeNews,
                                    Content = n.Content  // Nội dung
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
