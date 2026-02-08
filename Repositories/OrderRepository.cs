using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Orders.FindAsync(id);
            if (result != null)
            {
                _context.Orders.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
			return await _context.Orders.ToListAsync();
		}

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
			ArgumentNullException.ThrowIfNull(order, nameof(order));

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
		}
    }
}
