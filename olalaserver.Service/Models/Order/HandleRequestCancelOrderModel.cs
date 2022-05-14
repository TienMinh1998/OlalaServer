using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Order
{
    public class HandleRequestCancelOrderModel
    {
        public int ID { get; set; }
        public int IsCancel { get; set; }
    }
}
