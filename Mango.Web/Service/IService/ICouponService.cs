using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        public Task<ResponseDto?> GetCouponAsync(string couponCode);
        public Task<ResponseDto?> GetAllCouponsAsync();
        public Task<ResponseDto?> GetCouponByIdAsync(int id);
        public Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);
        public Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
        public Task<ResponseDto?> DeleteCouponAsync(int id);

    }
}
