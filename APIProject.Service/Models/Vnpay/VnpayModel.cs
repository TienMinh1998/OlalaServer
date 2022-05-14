using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Vnpay
{
    public class VnpViewModel
    {
        public void getVnpModel(string TxnRef, string money, string time, string type, string url = "")
        {
            vnp_TxnRef = TxnRef;
            vnp_money = money;
            vnp_time = time;
            vnp_type = type;
            vnp_url = url;
        }
        public string vnp_TxnRef { get; set; }
        public string vnp_money { get; set; }
        public string vnp_time { get; set; }
        public string vnp_type { get; set; }
        public string vnp_url { get; set; }
    }
    public class VNPayOutputModel
    {
        public string Message { get; set; }
        public string RspCode { get; set; }
        public VNPayOutputModel GetPayOutputModel(string Message, string ResponseCode)
        {
            VNPayOutputModel vnp = new VNPayOutputModel();
            vnp.Message = Message;
            vnp.RspCode = ResponseCode;
            return vnp;
        }

    }
    public class CreateOrderVNPayOutputModel
    {
        public int ID { get; set; }
        public string Url { get; set; }
    }
}
