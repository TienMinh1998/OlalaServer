using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Service.Utils;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountNotificationNotRead(int CusID)
        {
            try
            {
                return await DbContext.Notifications.Where(x => x.CustomerID.Equals(CusID) && x.Viewed.Equals(SystemParam.NOTI_NOT_VIEWD)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountNotificationNotRead(List<int> ListNotificationType)
        {
            try
            {
                return await DbContext.Notifications.Where(x => x.IsAdmin.Equals(SystemParam.ACTIVE) && ListNotificationType.Contains(x.Type) && x.Viewed.Equals(SystemParam.NOTI_NOT_VIEWD)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
