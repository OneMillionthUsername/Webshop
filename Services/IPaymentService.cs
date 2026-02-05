using Webshop.Models;

namespace Webshop.Services
{
    public interface IPaymentService
    {
        Task<Payment> ProcessStripePaymentAsync(string token, decimal amount, int orderId);
        Task<Payment> ProcessPayPalPaymentAsync(string paymentId, decimal amount, int orderId);
        Task<bool> VerifyPaymentAsync(string paymentReference);
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<bool> RefundPaymentAsync(int paymentId);
        Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(int orderId);
    }
}
