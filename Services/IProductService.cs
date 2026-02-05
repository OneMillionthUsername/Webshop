using Webshop.Models;

namespace Webshop.Services
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<bool> CheckInventoryAsync(int productId, int quantity);
        Task<int> GetAvailableStockAsync(int productId);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
    }
}
