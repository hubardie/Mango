using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        public Task<ResponseDto?> GetAllProductsAsync();
        public Task<ResponseDto?> GetProductByIdAsync(int id);
        public Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        public Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
        public Task<ResponseDto?> DeleteProductAsync(int id);

    }
}
