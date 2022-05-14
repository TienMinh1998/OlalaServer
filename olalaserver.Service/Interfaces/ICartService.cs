using APIProject.Domain.Models;
using APIProject.Service.Interface;
using APIProject.Service.Models;
using APIProject.Service.Models.Cart;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface ICartService : IServices<Cart>
    {
        /// <summary>
        /// Thêm vào Giỏ hàng :
        /// </summary>
        /// <param name="CusID"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        Task<JsonResultModel> AddCart(int CusID, int ProductID, int CusTypeID, int quantity);
        Task<JsonResultModel> BuyNow(int CusID, int ProductID, int CusTypeID, int quantity);
        Task<JsonResultModel> GetCart(int CusID,int? Type);
        Task<JsonResultModel> GetCartCount(int CusID);
        Task<JsonResultModel> DeleteCart(int cusID, DeleteCartModel model);
        Task<JsonResultModel> UpdateCart(int cusID, UpdateCartModel model);
        Task<bool> CheckCartChange(int CusID);
    }

}
                