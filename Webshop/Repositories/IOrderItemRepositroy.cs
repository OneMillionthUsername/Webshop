using Webshop.Models;

namespace Webshop.Repositories
{
	public interface IOrderItemRepository
	{
		Task<OrderItem?> GetByIdAsync(int id);
		Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
		Task<IEnumerable<OrderItem>> GetByProductVariantIdAsync(int productVariantId);
		Task<IEnumerable<OrderItem>> GetCartItemsAsync();
		Task<OrderItem> AddAsync(OrderItem orderItem);
		Task<OrderItem> UpdateAsync(OrderItem orderItem);
		Task<bool> DeleteAsync(int id);
	}
}
