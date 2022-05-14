using APIProject.Common.Models.News;
using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface INewsService : IServices<News>
    {
        Task<JsonResultModel> GetListNews(int page, int limit, string TitleBanner, int? Type, int? TypeNews, int? status, string fromDate, string toDate);
        Task<JsonResultModel> AddNews(NewAddInputModel model);
        Task<JsonResultModel> GetNewsDetail(int NewID);
        Task<JsonResultModel> GetNewsPartner();
        Task<JsonResultModel> UpdateNews(UpdateNewsModel model);
        Task<JsonResultModel> ChangeStatusNews(int ID);
        Task<JsonResultModel> DeleteNews(int NewsID);

    }
}
