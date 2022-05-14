using APIProject.Service.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Notification : BaseModel
    {
        public int? CustomerID { get; set; }
        public string Content { get; set; }
        public int Viewed { get; set; } = SystemParam.NOTI_NOT_VIEWD; // Đã xem
        public int Type { get; set; }
        public int? OrderID { get; set; }
        public int? NewsID { get; set; }
        public int? ProductID { get; set; }
        public int IsAdmin { get; set; } 
        public string CustomerPhone { get; set; }
        public string ProductCode { get; set; }
    }
}
