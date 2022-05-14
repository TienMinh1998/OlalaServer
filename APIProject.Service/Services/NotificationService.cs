using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Notification;
using APIProject.Service.Utils;
using AutoMapper;
using PagedList.Core;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly IHub _sentryHub;
        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, IHub sentryHub, IRolePermissionRepository rolePermissionRepository) : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _sentryHub = sentryHub;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public List<int> ConvertPermissionsToNoificationTypes(List<int> listPermission)
        {
            try
            {
                var listNotification = new List<int>();
                if (listPermission.Contains(SystemParam.PERMISSION_TYPE_ALL))
                {
                    listNotification.Add(SystemParam.NOTIFICATION_TYPE_ORDER);
                    listNotification.Add(SystemParam.NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING);
                    listNotification.Add(SystemParam.NOTIFICATION_TYPE_REQUEST_ROLE);
                }
                else
                {
                    if (listPermission.Contains(SystemParam.PERMISSION_TYPE_CUSTOMER))
                    {
                        listNotification.Add(SystemParam.NOTIFICATION_TYPE_REQUEST_ROLE);
                    }
                    if (listPermission.Contains(SystemParam.PERMISSION_TYPE_ORDER))
                    {
                        listNotification.Add(SystemParam.NOTIFICATION_TYPE_ORDER);
                    }
                    if (listPermission.Contains(SystemParam.PERMISSION_TYPE_PRODUCT))
                    {
                        listNotification.Add(SystemParam.NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING);
                    }
                }
                return listNotification;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResultModel> CountNotificationNotRead(int CusID)
        {
            try
            {
                var model = await _notificationRepository.CountNotificationNotRead(CusID);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> CountNotificationNotReadAdmin(int RoleID)
        {
            try
            {
                var listPermission = await _rolePermissionRepository.GetListRolePermission(RoleID);
                var listNotificationType = ConvertPermissionsToNoificationTypes(listPermission);
                var model = await _notificationRepository.CountNotificationNotRead(listNotificationType);
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }

        public async Task CreateNotification(Customer cus, string Content, int Type, int? OrderID, int? NewsID)
        {
            try
            {
                var model = new Notification
                {
                    CustomerID = cus.ID,
                    Content = Content,
                    Type = Type,
                    OrderID = OrderID,
                    NewsID = NewsID,
                    IsAdmin = SystemParam.NOTI_CUSTOMER,
                };
                await _notificationRepository.AddAsync(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);

            }
        }
        public async Task CreateNotification(IList<Customer> ListCus, string Content, int Type, int? OrderID, int? NewsID, int? ProductID)
        {
            try
            {
                List<Notification> model = new List<Notification>();
                foreach (var item in ListCus)
                {
                    var noti = new Notification
                    {
                        CustomerID = item.ID,
                        Content = Content,
                        Type = Type,
                        OrderID = OrderID,
                        NewsID = NewsID,
                        ProductID = ProductID,
                        IsAdmin = SystemParam.NOTI_CUSTOMER,
                    };
                    model.Add(noti);
                }
                await _notificationRepository.AddManyAsync(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);

            }
        }
        public async Task CreateNotificationAdmin(string Content, int Type, int? OrderID, int? NewsID)
        {
            try
            {
                var model = new Notification
                {
                    Content = Content,
                    Type = Type,
                    OrderID = OrderID,
                    NewsID = NewsID,
                    IsAdmin = SystemParam.NOTI_ADMIN,
                };
                await _notificationRepository.AddAsync(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);

            }
        }
        public async Task<JsonResultModel> GetListNotification(int Page, int Limit, int CusID)
        {
            try
            {
                var model = await _notificationRepository.GetAllPagedListAsync(Page, Limit, x => x.CustomerID.Equals(CusID), source => source.OrderByDescending(x => x.ID));
                return JsonResponse.Success(model);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> GetListNotificationAdmin(int Page, int Limit, int RoleID)
        {
            try
            {
                var listPermission = await _rolePermissionRepository.GetListRolePermission(RoleID);
                var listNotificationType = ConvertPermissionsToNoificationTypes(listPermission);
                var model = await _notificationRepository.GetAllPagedListAsync(Page, Limit, x => x.IsAdmin.Equals(SystemParam.NOTI_ADMIN) && listNotificationType.Contains(x.Type), source => source.OrderByDescending(x => x.ID));
                var data = new DataPagedListModel
                {
                    Page = Page,
                    Limit = Limit,
                    TotalItemCount = model.TotalItemCount,
                    Data = model
                };
                return JsonResponse.Success(data);
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ReadAllNotification(int CusID)
        {
            try
            {
                var model = await _notificationRepository.GetAllAsync(x => x.CustomerID.Equals(CusID) && x.Viewed.Equals(SystemParam.NOTI_NOT_VIEWD));
                foreach (var item in model)
                {
                    item.Viewed = SystemParam.NOTI_VIEWD;
                    await _notificationRepository.UpdateAsync(item);
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ReadAllNotificationAdmin(int RoleID)
        {
            try
            {
                var listPermission = await _rolePermissionRepository.GetListRolePermission(RoleID);
                var listNotificationType = ConvertPermissionsToNoificationTypes(listPermission);
                var model = await _notificationRepository.GetAllAsync(x => x.IsAdmin.Equals(SystemParam.ACTIVE) && listNotificationType.Contains(x.Type) && x.Viewed.Equals(SystemParam.NOTI_NOT_VIEWD));
                foreach (var item in model)
                {
                    item.Viewed = SystemParam.NOTI_VIEWD;
                    await _notificationRepository.UpdateAsync(item);
                }
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
        public async Task<JsonResultModel> ReadNotification(int ID)
        {
            try
            {
                var model = await _notificationRepository.GetFirstOrDefaultAsync(x => x.ID.Equals(ID));
                model.Viewed = SystemParam.NOTI_VIEWD;
                await _notificationRepository.UpdateAsync(model);
                return JsonResponse.Success();
            }
            catch (Exception ex)
            {
                _sentryHub.CaptureException(ex);
                return JsonResponse.ServerError();
            }
        }
    }
}
