using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Repositories
{
	public class OrderItemRepository : IOrderItemRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderItemRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<OrderItem?> GetByIdAsync(int id)
		{
			return await _context.OrderItems
				.Include(oi => oi.ProductVariant)
				.FirstOrDefaultAsync(oi => oi.Id == id);
		}

		public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
		{
			return await _context.OrderItems
				.Include(oi => oi.ProductVariant)
				.Where(oi => oi.OrderId == orderId)
				.ToListAsync();
		}

		public async Task<IEnumerable<OrderItem>> GetByProductVariantIdAsync(int productVariantId)
		{
			return await _context.OrderItems
				.Where(oi => oi.ProductVariantId == productVariantId)
				.ToListAsync();
		}
	}
}
