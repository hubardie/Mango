using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IShoppingCartService
    {
        public Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        public Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        public Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        public Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);



    }
}
