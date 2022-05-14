using APIProject.Common.Models.News;
using APIProject.Domain.Models;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Sentry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class NewsService : BaseService<News>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHub _sentryHub;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly ICustomerRepository _customerRepository;

        public NewsService(INewsRepository newsRepository, IHub sentry = null, ICustomerRepository customerRepository = null, IPushNotificationService pushNotificationService = null) : base(newsRepository)
        {
            _newsRepository = newsRepository;
            _sentryHub = sentry;
            _customerRepository = customerRepository;
            _pushNotificationService = pushNotificationService;
        }
        public async Task<JsonResultModel> AddNews(NewAddInputModel model)
        {
            try
            {
                News news = new News()
                {
                    Title = model.Title,
                    Type = model.Type,
                    TypeNews = model.TypeNews,
                    Content = model.Content,
                    UrlImage = model.UrlImage
                };

                await _newsRepository.AddAsync(news);
                if (model.SentNotification == true)
                {
                    try
                    {
                        var customers = await _customerRepository.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.Status.Equals(SystemParam.ACTIVE));
                        //Thread ThreadNotificationToCustomer = new Thread(() =>
                        //{
                        //    _pushNotificationService.PushNotification(customers, SystemParam.NOTIFICATION_TYPE_NEWS, model.Title, null, null);
                        //});
                        //ThreadNotificationToCustomer.Start();
                        await _pushNotificationService.PushNotification(customers, SystemParam.NOTIFICATION_TYPE_NEWS, model.Title, news.ID, null,null);
                    }
                    catch (Exception ex)
                    {
                        _sentryHub.CaptureException(ex);
                    }
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<JsonResultModel> UpdateNews(UpdateNewsModel model)
        {
            try
            {
                var news = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(model.ID));
                // Xử lý UpdateNew :
                if (news == null) return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS);
                news.Title = model.Title;
                news.Status = model.Status;
                news.TypeNews = model.TypeNews;
                news.Content = model.Content;
                news.Type = model.Type;
                news.UrlImage = model.URLImage;
                await _newsRepository.UpdateAsync(news);
                if (model.SentNotification == true)
                {
                    try
                    {
                        var customers = await _customerRepository.GetAllAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.Status.Equals(SystemParam.ACTIVE));
                        //ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                        //{
                        //    _pushNotificationService.PushNotification(customers, SystemParam.NOTIFICATION_TYPE_NEWS, model.Title, null, null);
                        //}));
                        await _pushNotificationService.PushNotification(customers, SystemParam.NOTIFICATION_TYPE_NEWS, model.Title, news.ID, null,null);
                    }
                    catch (Exception ex)
                    {
                        _sentryHub.CaptureException(ex);
                    }
                }
                return JsonResponse.Success(news);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResultModel> GetListNews(int page, int limit, string TitleBanner, int? Type, int? TypeNews, int? status, string fromDate, string toDate)
        {
            try
            {
                var list = await _newsRepository.GetListNews(page, limit, TitleBanner, Type, TypeNews, status, fromDate, toDate);
                DataPagedListModel dataPagedListModel = new DataPagedListModel()
                {
                    Data = list,
                    Limit = limit,
                    Page = page,
                    TotalItemCount = list.TotalItemCount
                };

                return JsonResponse.Success(dataPagedListModel);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetNewsDetail(int NewID)
        {
            try
            {
                var news = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(NewID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (news == null) return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS);  // không tìm thấy tin
                return JsonResponse.Success(news);     // Nếu có tìm thấy tin 
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> DeleteNews(int NewsID)
        {
            try
            {
                var news = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(NewsID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (news == null) return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS);
                news.IsActive = SystemParam.ACTIVE_FALSE;
                var response = await _newsRepository.UpdateAsync(news);
                return JsonResponse.Success(response);
            }
            catch (Exception Ex)
            {
                _sentryHub.CaptureException(Ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ChangeStatusNews(int ID)
        {
            try
            {
                News news = await _newsRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID) && x.IsActive.Equals(SystemParam.ACTIVE));
                if (news == null) JsonResponse.Response(SystemParam.ERROR, SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS, "");
                if (news.Status == SystemParam.ACTIVE_FALSE)
                {
                    news.Status = SystemParam.ACTIVE;
                }
                else
                {
                    news.Status = SystemParam.ACTIVE_FALSE;
                }
                await _newsRepository.UpdateAsync(news);
                return JsonResponse.Success();

            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }

        }

        public async Task<JsonResultModel> GetNewsPartner()
        {
            try
            {
                var news = await _newsRepository.GetFirstOrDefaultAsync(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.TypeNews.Equals(SystemParam.TYPE_PARTNER));
                if (news == null) return JsonResponse.Error(SystemParam.ERROR_CODE_NOT_FOUND_NEWS, SystemParam.MESSAGE_CODE_NOT_FOUND_NEWS);  // không tìm thấy tin
                return JsonResponse.Success(news);     // Nếu có tìm thấy tin 
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
