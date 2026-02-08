using Webshop.Models;

namespace Webshop.Services
{
    public interface ICartService
    {
        Task AddToCartAsync(int productId, int quantity, int? variantId = null);
        Task RemoveFromCartAsync(int cartItemId);
        Task UpdateQuantityAsync(int cartItemId, int quantity);
        Task<IEnumerable<OrderItem>> GetCartItemsAsync();
        Task ClearCartAsync();
        Task<decimal> CalculateCartTotalAsync();
        Task<bool> ValidateCartAsync();
    }
}
