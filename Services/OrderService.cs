using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Customer customer, IEnumerable<OrderItem> orderItems, decimal totalAmount)
        {
            var order = new Order
            {
                UserId = customer.Id.ToString(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Items = orderItems.ToList()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == customerId.ToString())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            // Status über Shadow Property oder neues Property setzen
            _context.Entry(order).Property("Status").CurrentValue = status;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            await UpdateOrderStatusAsync(orderId, "Cancelled");
            return true;
        }

        public async Task<string> GenerateInvoiceAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            
            // Vereinfacht: Invoice-Generierung (PDF-Bibliothek wie QuestPDF verwenden)
            var invoicePath = $"Invoices/Invoice_{orderId}_{DateTime.UtcNow:yyyyMMdd}.pdf";
            
            // TODO: Implementierung mit PDF-Generator
            // await GeneratePdfAsync(order, invoicePath);

            return invoicePath;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
