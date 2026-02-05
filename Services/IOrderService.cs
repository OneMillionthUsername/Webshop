using Webshop.Models;

namespace Webshop.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Customer customer, IEnumerable<OrderItem> orderItems, decimal totalAmount);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<string> GenerateInvoiceAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}
