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
				.OrderBy(oi => oi.Id)
				.ToListAsync();
		}

		public async Task<IEnumerable<OrderItem>> GetByProductVariantIdAsync(int productVariantId)
		{
			return await _context.OrderItems
				.Where(oi => oi.ProductVariantId == productVariantId)
				.ToListAsync();
		}

		public async Task<IEnumerable<OrderItem>> GetCartItemsAsync()
		{
			return await _context.OrderItems
				.Where(oi => oi.OrderId == null)
				.ToListAsync();
		}

		public async Task<OrderItem> AddAsync(OrderItem orderItem)
		{
			await _context.OrderItems.AddAsync(orderItem);
			await _context.SaveChangesAsync();
			return orderItem;
		}

		public async Task<OrderItem> UpdateAsync(OrderItem orderItem)
		{
			_context.OrderItems.Update(orderItem);
			await _context.SaveChangesAsync();
			return orderItem;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var item = await _context.OrderItems.FindAsync(id);
			if (item == null)
				return false;

			_context.OrderItems.Remove(item);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
