using APIProject.Common.Models.Notification;
using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiExplorerSettings(GroupName = "Web")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        /// <summary>
        /// lấy ra tất cả các thông báo có phân trang 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetNotifications")]
        [Authorize]
        public async Task<JsonResultModel> GetNotifications(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.LIMIT_DEFAULT)
        {
            var user = (User)HttpContext.Items["Payload"];
            return await _notificationService.GetListNotificationAdmin(page, limit,user.RoleID);
        }

        /// <summary>
        /// Đọc thông báo
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPut("ReadNotification/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ReadNotification(int ID)
        {
            return await _notificationService.ReadNotification(ID);
        }
        /// <summary>
        /// Số thông báo chưa đọc
        /// </summary>
        /// <returns></returns>
        [HttpGet("CountNotificationNotRead")]
        [Authorize]
        public async Task<JsonResultModel> CountNotificationNotRead()
        {
            var user = (User)HttpContext.Items["Payload"];
            return await _notificationService.CountNotificationNotReadAdmin(user.RoleID);
        }
        /// <summary>
        /// Đọc tất cả thông báo
        /// </summary>
        /// <returns></returns>
        [HttpPut("ReadAllNotification")]
        [Authorize]
        public async Task<JsonResultModel> ReadAllNotification()
        {
            var user = (User)HttpContext.Items["Payload"];
            return await _notificationService.ReadAllNotificationAdmin(user.RoleID);
        }

    }
}
