using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface ISocketService
    {
        Task PushSocket(int type, string content, int? orderID, string cusPhone, string productCode);
    }
}
