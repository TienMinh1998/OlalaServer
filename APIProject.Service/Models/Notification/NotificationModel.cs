using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Notification
{
    public class NotificationModel
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int Viewed { get; set; }
        public int Type { get; set; }
        public int? OrderID { get; set; }
        public int? NewsID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
