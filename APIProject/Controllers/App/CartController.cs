

/*-----------------------------------
 * Author   : NGuyễn Viết Minh Tiến
 * DateTime : 29/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Status   : Đã hoàn thiện
 * Content  : Controller Cart App  
 * ----------------------------------*/


using APIProject.Domain.Models;
using APIProject.Middleware;
using APIProject.Service.Interface;
using APIProject.Service.Interfaces;
using APIProject.Service.Models;
using APIProject.Service.Models.Cart;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APIProject.Controllers.App
{
    [Route("api/app/[controller]")]
    [ApiExplorerSettings(GroupName = "App")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICustomerService _customerService;

        public CartController(ICartService cartService, ICustomerService customerService)
        {
            _cartService = cartService;
            _customerService = customerService;
        }

        /// <summary>
        /// Lấy thông tin giỏ hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCart")]
        [Authorize]
        public async Task<JsonResultModel> GetCarts()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _cartService.GetCart(cus.ID,SystemParam.TYPE_BASIC_ITEM);
        }
        /// <summary>
        /// Lấy số lượng sản phẩm trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCartCount")]
        [Authorize]
        public async Task<JsonResultModel> GetCartCount()
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _cartService.GetCartCount(cus.ID);
        }
        /// <summary>
        /// Thêm vào giỏ hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddCart")]
        [Authorize]
        public async Task<JsonResultModel> AddCart([FromBody] AddCartModel model)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            return await _cartService.AddCart(cus.ID, model.ProductID, cus.CustomerTypeID, model.quantity);
        }

        /// <summary>
        /// Cập nhật giỏ hàng 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPut("UpdateCart")]
        public async Task<JsonResultModel> UpdateCart([FromBody] UpdateCartModel model)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            if (cus == null) return JsonResponse.ServerError();
            return await _cartService.UpdateCart(cus.ID, model);
        }
        /// <summary>
        /// Xóa giỏ hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("DeleteCart")]
        public async Task<JsonResultModel> DeleteCart([FromBody] DeleteCartModel model)
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            if (cus == null) return JsonResponse.ServerError();
            return await _cartService.DeleteCart(cus.ID, model);
        }


        /// <summary>
        ///  Mua ngay
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("BuyNow")]
        public async Task<JsonResultModel> BuyNow([FromBody] AddCartModel model )
        {
            var cus = (Customer)HttpContext.Items["Payload"];
            if (cus == null) return JsonResponse.Response(SystemParam.ERROR,SystemParam.TOKEN_INVALID, SystemParam.MESSAGE_TOKEN_INVALID,"");     
            return await _cartService.BuyNow(cus.ID, model.ProductID, cus.CustomerTypeID, model.quantity);
        }

    }
}
