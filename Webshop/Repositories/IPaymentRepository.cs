using Webshop.Models;

namespace Webshop.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<IEnumerable<Payment>> GetByStatus(string Status);
        Task<IEnumerable<Payment>> GetByPaymentMethod(string PaymentMethod);
        Task<Payment?> GetByTransactionId(int transactionId);
        Task<Payment?> GetByDate(DateTime? date);
        Task<(int Id, string PaymentMethod, int Count)> GetPaymentMethodAndCount(string paymentMethod);
        Task<Payment> UpdateAsync(Payment payment);
        Task<Payment> AddAsync(Payment payment);
        Task<bool> DeleteAsync(int id);
    }
}
