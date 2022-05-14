using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Order
{
   public class PaymentOrderModel
    {
        public int OrderID { get; set; }
        public int UserPoint { get; set; }
        public int paymentType { get; set; }
    }
}
