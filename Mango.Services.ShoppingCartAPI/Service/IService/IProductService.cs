using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Service.Iservice
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();

    }
}
