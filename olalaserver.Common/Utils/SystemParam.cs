
/*-----------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 13/12/2021
 * Edit     : Đã chỉnh sửa nhiều
 * Content  :Định nghĩa các thông báo và mmax lỗi cho người dùng
 * ----------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Utils
{

    public class SystemParam
    {

        #region -- CONSTANT --
        // Notification
        public const string NOTI_TITLE = "Gia Anh Food thông báo";
        public const string APP_ID = "a921d7f6-07b1-4aba-816b-1032e8c61a2e";
        public const string Authorization = "Basic :MmZmZjkyZDctNzMwOS00OTQzLTg5NjctYjBiY2VjOGE4NDM5";
        public const string ANDROID_CHANNEL_ID = "4a8a36b0-af89-41cd-9288-60ef1f6e4065";
        public const string URL_ONESIGNAL = "https://onesignal.com/api/v1/notifications";
        public const string URL_BASE_https = "Basic ://onesignal.com/api/v1/notifications";

        public const int NOTI_VIEWD = 1;
        public const int NOTI_NOT_VIEWD = 0;

        public const int NOTI_ADMIN = 1;
        public const int NOTI_CUSTOMER = 0;

        public const int NOTIFICATION_TYPE_ORDER = 1; // Thông báo đơn hàng
        public const string NOTIFICATION_TYPE_ORDER_NEW_STR = "Đơn hàng {0} cần báo giá vận chuyển";
        public const string NOTIFICATION_TYPE_ORDER_CONFIRM_STR = "Đơn hàng {0} cần được xác nhận";
        public const string NOTIFICATION_TYPE_ORDER_REQUEST_CANCEL_STR = "Đơn hàng {0} vừa có yêu cầu hủy đơn hàng ";
        public const string NOTIFICATION_TYPE_ORDER_COMPLAIN_STR = "Đơn hàng {0} vừa có yêu cầu khiếu nại ";
        public const int NOTIFICATION_TYPE_BONUS_POINT = 2; // Thông báo tặng điểm
        public const int NOTIFICATION_TYPE_NEWS = 3; // Thông báo tin tức
        public const int NOTIFICATION_TYPE_CONFIRM_ROLE = 4; // Thông báo duyệt khách hàng
        public const int NOTIFICATION_TYPE_REQUEST_ROLE = 5; // Thông báo yêu cầu duyệt khách hàng
        public const string NOTIFICATION_TYPE_REQUEST_ROLE_STR = "Khách hàng {0} vừa yêu cầu trở thành doanh nghiệp";
        public const int NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING = 6; // Thông báo cảnh báo lượng tồn kho tối thiểu
        public const string NOTIFICATION_TYPE_PRODUCT_STORAGE_WARNING_STR = "Sản phẩm {0} sắp hết số lượng tồn trong kho "; // Thông báo cảnh báo lượng tồn kho tối thiểu
        public const int NOTIFICATION_TYPE_PRODUCT_NEW = 7; // Thông báo cảnh báo lượng tồn kho tối thiểu
        public const string NOTIFICATION_TYPE_PRODUCT_NEW_STR = "Vừa mới ra mắt sản phẩm {0}"; // Thông báo sản phẩm mới
        public const string NOTIFICATION_TYPE_CONFIRM_ROLE_STR = "Bạn đã được duyệt trở thành {0}";

        // Socket
        public const string URL_WEB_SOCKET = "http://api.zoco.winds.vn/api/v1/admin/notification/push-socket-GA";

        // Upload File
        public const string FILE_NAME = "Files";

        // VNPAY KEY
        public const string vnp_CodeSucces = "00";
        public const string vnp_Url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public const string vnp_TmnCode = "CRYSN1H9";
        public const string vnp_HashSecret = "";
        public const string vnp_Return_url = "http://giaanhfoods.winds.vn/VnPay/Index";

        public const string vnp_CodeSucces_Real = "00";
        public const string vnp_Url_Real = "https://pay.vnpay.vn/vpcpay.html";
        public const string vnp_TmnCode_Real = "XNKHH001";
        public const string vnp_HashSecret_Real = "DULIMMOBQUSJOYTIUUOWMNDWDPKGMDKY";
        public const string vnp_Return_url_Real = "http://mrfish.winds.vn/VnPay/Index";

        public const int PAGE_DEFAULT = 1;
        public const int LIMIT_DEFAULT = 10;
        public const int LIMIT_MAX_DEFAULT = 100;

        public const int ACTIVE = 1;
        public const int ACTIVE_FALSE = 0;

        public const int SORT_ASCENDING = 1;
        public const int SORT_DESCENDING = 2;

        public const string CONVERT_DATETIME = "dd/MM/yyyy";
        public const string CONVERT_DATETIME_HAVE_HOUR = "HH:mm dd/MM/yyyy";

        // OTP
        public const int OTP_MAX_QUANTITY = 5;

        // Token Type
        public const string TOKEN_TYPE_CUSTOMER = "1";
        public const string TOKEN_TYPE_USER = "2";

        // Product Type
        /// <summary>
        /// sản phẩm khuyến mại
        /// </summary>
        public const int PRODUCT_TYPE_SALE = 1;
        /// <summary>
        /// Sản phẩm hot 
        /// </summary>
        public const int PRODUCT_TYPE_HOT = 2;
        /// <summary>
        /// Sản phẩm bán chạy
        /// </summary>
        public const int PRODUCT_TYPE_TREND = 3;

        // Customer Type
        /// <summary>
        /// Khách hàng thường 
        /// </summary>
        public const int CUSTOMER_TYPE_NORMAL = 1;
        /// <summary>
        /// Đại lý  
        /// </summary>
        public const int CUSTOMER_TYPE_AGENT = 2;
        /// <summary>
        /// Siêu thị  
        /// </summary>
        public const int CUSTOMER_TYPE_SUPERMARKET = 3;
        /// <summary>
        /// Chợ xỉ
        /// </summary>
        public const int CUSTOMER_TYPE_MARKET = 4;
        /// <summary>
        /// Xuất ăn công nghiệp
        /// </summary>
        public const int CUSTOMER_TYPE_INDUSTRY = 5;
        /// <summary>
        /// Nhà hàng 
        /// </summary>
        public const int CUSTOMER_TYPE_RESTAURANT = 6;

        // Receive Address
        public const int RECEIVE_ADDRESS_DEFAULT = 1;
        public const int RECEIVE_ADDRESS_NOT_DEFAULT = 0;

        // Cart
        public const int QUANTITY_MAX = 9999;
        public const int TYPE_BUYNOW = 1;
        public const int TYPE_BASIC_ITEM = 0;

        // Order
        public const int STATUS_ORDER_QUOTE = 0;      // Báo giá vận chuyển
        public const int STATUS_ORDER_PENDING = 1;    // Chờ xác nhận
        public const int STATUS_ORDER_DELIVERING = 2; // Đang giao
        public const int STATUS_ORDER_DELIVERED = 3;   // Đã giao
        public const int STATUS_ORDER_COMPLETE = 4;   // Hoàn thành
        public const int STATUS_ORDER_CANCEL = -1;    // Hủy
        public const int STATUS_ORDER_COMPLAIN = -2;  // Khiếu nại
        public const int STATUS_ORDER_RETURN = -3;  // Trả hàng

        public const int STATUS_PAYMENT_NOT_PAID = 0; // Chưa thanh toán
        public const int STATUS_PAYMENT_PAID = 1; // Đã thanh toán

        public const int STATUS_QUOTE_NOT_QUOTED = 0; // Chưa báo giá
        public const int STATUS_QUOTE_QUOTED = 1; // Đã báo giá

        public const int ORDER_VNPAY_MIN_PRICE = 10000;

        public const int COMPLETE_ORDER_TIME = 3; // Thời gian hoàn thành đơn đã giao (ngày)

        public const string MESSAGE_PAID = "Đơn hàng đã được thanh toán";

        // PaymentType
        public const int PAYMENT_TYPE_CASH = 1; // Tiền mặt
        public const int PAYMENT_TYPE_TRANFER = 2; // Chuyển khoản
        public const int PAYMENT_TYPE_VNPAY = 3; // VNPAY


        // Member Point History

        public const int TYPE_ADD_POINT = 1;
        public const int TYPE_MINUS_POINT = 2;

        public const int TYPE_MPH_EARN_POINT = 1;
        public const int TYPE_MPH_USED_POINT = 2;
        public const int TYPE_MPH_ALL = 0;



        // Product Storage History
        public const int TYPE_PRODUCT_STORAGE_HISTORY_IMPORT = 1;
        public const int TYPE_PRODUCT_STORAGE_HISTORY_EXPORT = 2;

        // Export Storage
        public const string REASON_EXPORT_STORAGE_ORDER = "Xuất kho từ đơn hàng {0}";

        // Permission Type
        public const int PERMISSION_TYPE_ALL = 1;
        public const int PERMISSION_TYPE_OVERVIEW = 2;
        public const int PERMISSION_TYPE_CUSTOMER = 3;
        public const int PERMISSION_TYPE_CATEGORY = 4;
        public const int PERMISSION_TYPE_PRODUCT = 5;
        public const int PERMISSION_TYPE_ORDER = 6;
        public const int PERMISSION_TYPE_STORAGE = 7;
        public const int PERMISSION_TYPE_STATISTIC = 8;
        public const int PERMISSION_TYPE_NEWS = 9;
        public const int PERMISSION_TYPE_CONFIG = 10;
        public const int PERMISSION_TYPE_ROLE = 11;
        public const int PERMISSION_TYPE_USER = 12;




        #endregion
        #region -- API RESPONSE --

        // Default
        public const int SUCCESS = 1;
        public const int ERROR = 0;
        public const int SUCCESS_CODE = 200;
        public const string MESSAGE_SUCCESS = "Thành công";
        public const int ERROR_CODE = 501;
        public const string MESSAGE_ERROR = "Thất bại";
        public const string SERVER_ERROR = "Hệ thống đang bảo trì";
        public const int TOKEN_INVALID = 401;
        public const string MESSAGE_TOKEN_INVALID = "Đăng nhập để thực hiện chức năng này";
        public const int PERMISSION_INVALID = 402;
        public const string MESSAGE_PERMISSION_INVALID = "Bạn không có quyền thực hiện chức năng này";
        public const int TOKEN_ERROR = 403;
        public const string MESSAGE_TOKEN_ERROR = "Tài khoản của bạn đã đăng nhập ở nơi khác";

        // Login
        public const int ERROR_LOGIN_FIELDS_INVALID = 1;
        public const string MESSAGE_LOGIN_FIELDS_INVALID = "Vui lòng nhập tài khoản và mật khẩu";
        public const int ERROR_LOGIN_FAIL = 2;
        public const string MESSAGE_LOGIN_FAIL = "Tài khoản hoặc mật khẩu không đúng";

        // Register
        public const int ERROR_REGISTER_FIELDS_INVALID = 1;
        public const string MESSAGE_REGISTER_FIELDS_INVALID = "Vui lòng nhập đầy đủ thông tin bắt buộc";
        public const int ERROR_REGISTER_PHONE_INVALID = 2;
        public const string MESSAGE_REGISTER_PHONE_INVALID = "Số điện thoại không đúng định dạng";
        public const int ERROR_REGISTER_PHONE_EXIST = 3;
        public const string MESSAGE_REGISTER_PHONE_EXIST = "Số điện thoại đã tồn tại";
        public const int ERROR_REGISTER_EMAIL_INVALID = 4;
        public const string MESSAGE_REGISTER_EMAIL_INVALID = "Email không đúng định dạng";
        public const int ERROR_REGISTER_EMAIL_EXIST = 5;
        public const string MESSAGE_REGISTER_EMAIL_EXIST = "Email đã tồn tại";
        public const int ERROR_CODE_CODETAX_INVALID = 6;
        public const string MESSAGE_CODETAX_INVALID = "Mã số thuế đã được sử dụng";

        // Category
        public const int ERROR_CATEGORY_EXIST = 1;
        public const string MESSAGE_CATEGORY_EXIST = "Danh mục đã tồn tại";
        public const int ERROR_CATEGORY_PRODUCT_STILL_EXIST = 2;
        public const string MESSAGE_CATEGORY_PRODUCT_STILL_EXIST = "Không thể xóa danh mục khi vẫn còn sản phẩm cùng danh mục";
        public const int ERROR_CATEGORY_NOT_EXIST = 3;
        public const string MESSAGE_CATEGORY_NOT_EXIST = "Danh mục không tồn tại";

        // Product
        public const int CUSTOMER_TYPE_NOT_FOUND = -1;
        public const string MESSAGE_CUS_TYPE_NOT_FOUND = "Loại khách hàng không tồn tại";
        public const int ERROR_CREATE_PRODUCT_FIELDS_INVALID = 1;
        public const string MESSAGE_CREATE_PRODUCT_FIELDS_INVALID = "Vui lòng nhập đầy đủ thông tin bắt buộc";
        public const int ERROR_CREATE_PRODUCT_DUPLICATE_CODE = 2;
        public const string MESSAGE_CREATE_PRODUCT_DUPLICATE_CODE = "Mã sản phẩm đã tồn tại";
        public const int ERROR_PRODUCT_NOT_EXIST = 3;
        public const string MESSAGE_PRODUCT_NOT_EXIST = "Sản phẩm không tồn tại";

        // Receive Address 
        public const int ERROR_RECEIVE_ADDRESS_NOT_EXIST = 1;
        public const string MESSAGE_RECEIVE_ADDRESS_NOT_EXIST = "Địa chỉ nhận hàng không tồn tại";

        // Upload File
        public const int ERROR_FILE_NOT_FOUND = 1;
        public const string MESSAGE_FILE_NOT_FOUND = "Không tìm thấy ảnh tải lên ";

        //Product Item : 
        public const int ERROR_PRODUCTITEM_NOT_FOUND = 1;
        public const string MESSAGE_PRODUCTITEM_NOT_FOUND = "Sản phẩm không tồn tại";

        //Cart + Order
        public const int ERROR_CART_UPDATED = 1;
        public const string MESSAGE_CART_UPDATED = "Giỏ hàng đã bị thay đổi";
        public const int ERROR_CART_NOTFOUND = 2;
        public const string MESSAGE_NOTFOUND_ITEMCART = "Không tìm thấy sản phẩm trong giỏ hàng";
        public const int ERROR_CART_QUANTITY_LIMITED = 3;
        public const string MESSAGE_QUANTITY_ERROR = "Só lượng sản phẩm đã vượt quá 9999";
        public const int ERROR_CART_EMPTY = 4;
        public const string MESSAGE_CART_EMPTY = "Giỏ hàng đang trống";
        public const int ERROR_RECEIVE_ADDRESS_NOT_FOUND = 5;
        public const string MESSAGE_RECEIVE_ADDRESS_NOT_FOUND = "Thông tin nhận hàng không tồn tại";
        public const int ERROR_PRODUCT_CART_EXCEED_QUANTITY = 6;
        public const int ERROR_PRODUCT_CART_NOT_AVAILABLE = 7;
        public const int ERROR_NOT_FOUND_ORDER = 8;
        public const string MESSAGE_NOT_FOUND_ORDER = "Không tìm thấy đơn hàng ";

        // Change Status Order
        public const int ERROR_ORDER_ALREADY_COMPLETE = 1;
        public const string MESSAGE_ORDER_ALREADY_COMPLETE = "Đơn hàng đã được hoàn thành";
        public const int ERROR_ORDER_ALREADY_CANCEL = 2;
        public const string MESSAGE_ORDER_ALREADY_CANCEL = "Đơn hàng đã bị hủy";
        public const int ERROR_ORDER_NOT_QUOTE = 3;
        public const string MESSAGE_ORDER_NOT_QUOTE = "Đơn hàng chưa được báo giá vận chuyển";
        public const int ERROR_ORDER_ALREADY_PAID = 4;
        public const string MESSAGE_ORDER_ALREADY_PAID = "Đơn hàng đã được thanh toán";
        public const int ERROR_ORDER_VNPAY_LIMIT_MONEY = 5;
        public const string MESSAGE_ORDER_VNPAY_LIMIT_MONEY = "Đơn hàng thanh toán VNPAY phải có tổng giá trị lớn hơn hoặc bằng 10.000đ , vui lòng chọn hình thức thanh toán khác !";
        public const int ERROR_ORDER_COMPLAIN_NO_IMAGE = 6;
        public const string MESSAGE_ORDER_COMPLAIN_NO_IMAGE = "Vui lòng gửi ảnh khiếu nại !";

        // Customer 
        public const int ERROR_NOT_FOUND_CUSTOMER = 604;
        public const string MESSAGE_NOT_FOUND_CUSTOMER = "Không tìm thấy khách hàng";
        public const string MESSAGE_LOCK_ACOUNT = "Khách hàng đang bị khóa tài khoản";
        public const int ERROR_LOCK_ACOUNT = -1;
        // Storage
        public const int ERROR_STORAGE_NOT_FOUND = 1;
        public const string MESSAGE_STORAGE_NOT_FOUND = "Kho không tồn tại";

        // Product Storage
        public const int ERROR_PRODUCT_STORAGE_NOT_FOUND = 1;
        public const string MESSAGE_PRODUCT_STORAGE_NOT_FOUND = "Sản phẩm trong kho không tồn tại";

        // Storage Import
        public const int ERROR_STORAGE_IMPORT_NOT_FOUND = 1;
        public const string MESSAGE_STORAGE_IMPORT_NOT_FOUND = "Thông tin nhập kho không tồn tại";

        // Storage Export
        public const int ERROR_STORAGE_EXPORT_NOT_FOUND = 1;
        public const string MESSAGE_STORAGE_EXPORT_NOT_FOUND = "Thông tin xuất kho không tồn tại";
        public const int ERROR_STORAGE_EXPORT_PRODUCT_NOT_FOUND = 2;
        public const string MESSAGE_STORAGE_EXPORT_PRODUCT_NOT_FOUND = "Sản phẩm không tồn tại trong kho";
        public const int ERROR_STORAGE_EXPORT_PRODUCT_EXCEED_QUANTITY = 3;
        public const string MESSAGE_STORAGE_EXPORT_PRODUCT_EXCEED_QUANTITY = "Số lượng xuất kho vượt quá số lượng tồn trong kho";
        public const int ERROR_STORAGE_EXPORT_PRODUCT_ORDER_EXCEED_QUANTITY = 4;
        public const string MESSAGE_STORAGE_EXPORT_PRODUCT_ORDER_EXCEED_QUANTITY = "{0} còn thiếu {1} sản phẩm để tạo phiếu xuất kho";

        // Payment Order
        public const int MEMBER_POINT_HISTORY_TYPE_COMPLETE_ORDER = 1; // Hoàn thành đơn hàng
        public const int MEMBER_POINT_HISTORY_TYPE_USE_POINT = 2;      // Sử dụng điểm thành công  
        public const string MESSAGE_NOT_ENOUGH_POINT = "Bạn không đủ điểm";
        public const string MESSAGE_PAYMENT_NOT_USE_POINT = "Thanh Toán Không sử dụng điểm";
        public const string MESSAGE_PAYMENT_USE_POINT = "Thanh toán sử dụng điểm";


        // Password :
        public const int ERROR_CODE_CUSOTMER_LOCK = 1;
        public const string MESSAGE_LOCK_CUSTOMER = "Tài khoản khách hàng đang bị khóa";
        public const string MESSAGE_NOT_CONFIRM_OTP = "Vui Lòng xác nhận OTP trước khi đổi MK";
        public const int CODE_NOT_CONFIRM_OTP = 2;


        // OTP 
        public const int ERROR_CODE_INVALID_OTP = -1;
        public const string MESSAGE_CODE_INVALID_OTP = "Mã OTP Không hợp lệ";
        public const int ERROR_CODE_UNCONFIRMED = -2;
        public const string MESSAGE_UNCONFIRMED = "Bạn chưa xác nhận OTP";
        public const string MESSAGE_PHONE_NOT_FOUND = "Số điện thoại chưa được đăng kí";
        public const int ERROR_CODE_PHONE_NOT_FOUND = -3;
        public const int ERROR_OTP_MAX_QUANTITY_EXCEED = -4;
        public const string MESSAGE_OTP_MAX_QUANTITY_EXCEED = "Bạn đã vượt quá số lần gửi mã OTP trong hôm nay";

        // VnPay 
        public const string TRANSACTION_SUCCESS = "Giao dịch thành công";
        public const string TRANSACTION_FAIL = "Giao dịch thất bại";
        public const string customer_success = "GiaAnhFoods://success/";
        public const string customer_failed = "GiaAnhFoods://failed/";

        // New 
        public const int STATUS_SAVE_DARFT = 0;   //  Lưu nháp
        public const int STATUS_POST_NEWS = 1;   //  Đăng bài

        public const int TYPE_BANNER = 0;     //  Kiểu Banner
        public const int TYPE_PARTNER = 1;     //  kiểu trả thành đối tác

        public const int ERROR_CODE_NOT_FOUND_NEWS = -1;
        public const string MESSAGE_CODE_NOT_FOUND_NEWS = "Không tìm thấy tin tức";
        public const string MESSAGE_WATCHED = "Tin này đã được đọc trước đó hoặc không tồn tại";



        // User
        public const int ERROR_USER_NOT_FOUND = 1;
        public const string MESSAGE_USER_NOT_FOUND = "Tài khoản không tồn tại";

        public const int ERROR_CHANGE_PASSWORD_WRONG = -1;
        public const string MESSAGE_CHANGE_PASSWORD_WRONG = "Mật khẩu cũ không đúng";

        // Role
        public const int ERROR_ROLE_NOT_FOUND = 1;
        public const string MESSAGE_ROLE_NOT_FOUND = "Phân quyền không tồn tại";
        public const int ERROR_ROLE_NAME_ALREADY_EXIST = 2;
        public const string MESSAGE_ROLE_NAME_ALREADY_EXIST = "Tên phân quyền đã tồn tại";
        public const int ERROR_ROLE_USER_STILL_EXIST = 3;
        public const string MESSAGE_ROLE_USER_STILL_EXIST = "Không thể xóa phân quyền khi vẫn tồn tại tài khoản thuộc phân quyền";

        #endregion

    }
}