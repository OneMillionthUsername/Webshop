using Webshop.Models;
using Webshop.Repositories;

namespace Webshop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(Customer customer, IEnumerable<OrderItem> orderItems, decimal totalAmount)
        {
            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Items = orderItems.ToList()
            };

            return await _orderRepository.AddAsync(order);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return orders.OrderByDescending(o => o.OrderDate);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _orderRepository.UpdateStatusAsync(orderId, status);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
                return false;

            await UpdateOrderStatusAsync(orderId, "Cancelled");
            return true;
        }

        public async Task<string> GenerateInvoiceAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);

            // Vereinfacht: Invoice-Generierung (PDF-Bibliothek wie QuestPDF verwenden)
            var invoicePath = $"Invoices/Invoice_{order.Id}_{DateTime.UtcNow:yyyyMMdd}.pdf";

            // TODO: Implementierung mit PDF-Generator
            // await GeneratePdfAsync(order, invoicePath);

            return invoicePath;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.OrderByDescending(o => o.OrderDate);
        }
    }
}
