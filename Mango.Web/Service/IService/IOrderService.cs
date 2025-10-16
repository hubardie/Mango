using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        public Task<ResponseDto?> CreateOrderAsync(CartDto carDto);
        public Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);
        public Task<ResponseDto?> ValidateStripeSession(int orderHeaderId);


    }
}
