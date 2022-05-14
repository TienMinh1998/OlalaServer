using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class Order :BaseModel
    {
        [StringLength(50)]
        public string Code { get; set; }
        public long TotalPrice { get; set; } // Tổng tiền
        public long? UsePoint { get; set; } // Sử dụng điểm 
        public long? ShipFee { get; set; } // Phí giao hàng
        public long ProductSumPrice { get; set; } // Tổng tiền hàng
        public double TotalWeight { get; set; } // Tổng trọng lượng
        [StringLength(100)]
        public string BuyerName { get; set; }
        [StringLength(15)]
        public string BuyerPhone { get; set; }            
        public string BuyerAddress { get; set; }
        public string Note { get; set;  }
        public string NoteComplain { get; set;  }
        public int? DeclineRequest { get; set; } // Yêu cầu hủy đơn hàng
        public string DeclineNote { get; set; } // Note từ chối yêu cầu 
        public int? PaymentType { get; set; } // Hình thức thanh toán
        public int PaymentStatus { get; set; } // Trạng thái thanh toán
        public int ShipQuoteStatus { get; set; } // Trạng thái báo giá vận chuyển
        public string UrlVnpay { get; set; }
        public int Status { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int ProvinceID { get; set; }
        public Province Province { get; set; }
        public int DistrictID { get; set; }
        public District District { get; set; }
        public int WardID { get; set; }
        public Ward Ward { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<OrderHistory> OrderHistories { get; set; }
        public ICollection<OrderComplainImage> OrderComplainImages { get; set; }
    }
}
