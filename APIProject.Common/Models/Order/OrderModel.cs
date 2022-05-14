using APIProject.Common.Models.Product;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.Models.Order
{
    public class CreateOrderModel
    {
        public string Note { get; set; }
        public List<int> CartID { get; set; }
        public int ReceiveAddressID { get; set; }
    }

    public class OrderWebModel
    {
        public int ID { get; set; }                // Id 
        public string Code { get; set; }      // Mã đơn hàng
        public string CustomerName { get; set; }   // Tên khách hàng
        public string Phone { get; set; }          // Số điện thoại
        public long TotalPrice { get; set; }         // Tổng tiền
        public int? PaymentType { get; set; }      // hình thức thanh toán
        public int Status { get; set; }       // Trạng thái của order 
        public int ShipQuoteStatus { get; set; }       // Trạng thái báo giá của order 
        public DateTime CreatedDate { get; set; }   // ngày tạo
        public int? DeclineRequest { get; set; } // Yêu cầu hủy đơn hàng
    }

    public class ListOrderModel
    {
        public OrderCountModel OrderCount { get; set; }
        public IPagedList<OrderModel> ListOrder { get; set; }
    }
    public class OrderCountModel
    {
        public int Quote { get; set; } // Báo giá vận chuyển
        public int Pending { get; set; } // Chờ xác nhận
        public int Delivering { get; set; } // Đang giao
        public int Delivered { get; set; } // Đã giao
        public int Complete { get; set; } // Hoàn thành
        public int Cancel { get; set; } // Hủy
        public int Complain { get; set; } // Khiếu nại
        public int Return { get; set; } // Trả hàng
    }

    public class OrderModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int? DeclineRequest { get; set; } // Yêu cầu hủy đơn hàng
        public List<ProductOrderModel> ListProduct { get; set; }
        public int ShipQuoteStatus { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }       
    }
    public class ProductOrderModel 
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

    }
    public class OrderDetailModel : OrderModel
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhone { get; set; }
        public string Note { get; set; }
        public int? PaymentType { get; set; }
        public long? ShipFee { get; set; }
        public long? UsePoint { get; set; }
        public long? ProductSumPrice { get; set; }
        public int? DeclineRequest { get; set; } 
        public string DeclineNote { get; set; }
        public string NoteComplain { get; set; }
        public List<string> ListComplainImage { get; set; }
        public List<HistoryOrderModel> ListHistoryOrder { get; set; }
    }
    public class HistoryOrderModel
    {
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
     
    }
}
