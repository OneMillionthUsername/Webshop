using Webshop.Models;

namespace Webshop.Repositories
{
	public interface IOrderItemRepository
	{
		Task<OrderItem?> GetByIdAsync(int id);
		Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
		Task<IEnumerable<OrderItem>> GetByProductVariantIdAsync(int productVariantId);
	}
}
