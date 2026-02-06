using Webshop.Dtos.Products;
using Webshop.Dtos.Categories;

namespace Webshop.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto);
        Task DeleteProductAsync(int productId);
        Task<bool> ProductExistsAsync(int productId);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<bool> CheckInventoryAsync(int productId, int quantity);
        Task<int> GetAvailableStockAsync(int productId);
        Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync();
    }
}
