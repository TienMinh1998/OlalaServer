using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Notification;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _NotificationService;
        private readonly IPushNotificationService _PushNotificationService;

        public NotificationController(INotificationService NotificationService, IPushNotificationService pushNotificationService)
        {
            _NotificationService = NotificationService;
            _PushNotificationService = pushNotificationService;
        }
        /// <summary>
        /// Danh sách thông báo
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        [HttpGet("GetListNotification")]
        [Authorize]
        public async Task<JsonResultModel> GetListNotification(int Page = SystemParam.PAGE_DEFAULT, int Limit = SystemParam.LIMIT_DEFAULT)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _NotificationService.GetListNotification(Page, Limit,cus.ID);
        }
        /// <summary>
        /// Đếm số thông báo chưa đọc
        /// </summary>
        /// <returns></returns>
        [HttpGet("CountNotificationNotRead")]
        [Authorize]
        public async Task<JsonResultModel> CountNotificationNotRead()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _NotificationService.CountNotificationNotRead(cus.ID);
        }
        /// <summary>
        /// Đọc thông báo
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("ReadNotification/{ID}")]
        [Authorize]
        public async Task<JsonResultModel> ReadNotification(int ID)
        {
            return await _NotificationService.ReadNotification(ID);
        }
        /// <summary>
        /// Đọc tất cả thông báo
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReadAllNotification")]
        [Authorize]
        public async Task<JsonResultModel> ReadAllNotification()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _NotificationService.ReadAllNotification(cus.ID);
        }
        /// <summary>
        /// Gửi thông báo
        /// </summary>
        /// <returns></returns>
        [HttpPost("SendNotification")]
        [Authorize]
        public async Task<JsonResultModel> SendNotification([FromBody] NotificationModel input)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _PushNotificationService.SendNotification(cus, input);
        }
    }
}
