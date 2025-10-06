using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();

    }
}
