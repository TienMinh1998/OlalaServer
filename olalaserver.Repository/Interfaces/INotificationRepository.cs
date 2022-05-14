using APIProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<int> CountNotificationNotRead(int CusID);
        Task<int> CountNotificationNotRead(List<int> ListNotificationType);
    }
}
