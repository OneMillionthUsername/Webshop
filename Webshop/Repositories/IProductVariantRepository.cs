using Webshop.Models;

namespace Webshop.Repositories
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id);
        Task<ProductVariant?> GetBySKUAsync(string sku);
        Task<ProductVariant> AddAsync(ProductVariant variant);
        Task<ProductVariant> UpdateAsync(ProductVariant variant);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<ProductVariant>> GetAllAsync();
        Task<IEnumerable<ProductVariant>> GetHighQuantity();
        Task<IEnumerable<ProductVariant>> GetLowQuantity();
        Task<IEnumerable<ProductVariant>> GetEmpty();
        Task<IEnumerable<ProductVariant>> GetByAttributeAsync(string attributeName, string attributeValue);
    }
}
