using APIProject.Service.Library;
using APIProject.Service.Models.Vnpay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IVnpayService
    {
        Task<string> GetUrl(int orderID);
        Task<VNPayOutputModel> GetVnpIpn(VnpOutputModel vnp);
        Task<VnpViewModel> GetVnpReturn(VnpOutputModel vnp);
    }
}
