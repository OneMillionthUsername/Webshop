using Webshop.Models;

namespace Webshop.Repositories
{
    public interface IPaymentRepository
    {
        // Basis CRUD
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

        // Bereichsabfragen
        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetByAmountRangeAsync(decimal minAmount, decimal maxAmount);

        // Paginierung
        Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPagedByStatusAsync(string status, int pageNumber, int pageSize);

        // Aggregationen und Statistiken
        Task<decimal> GetTotalAmountByStatusAsync(string status);
        Task<decimal> GetTotalAmountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetCountByStatusAsync(string status);
        Task<decimal> GetAverageAmountAsync();
        Task<Dictionary<string, int>> GetPaymentMethodStatisticsAsync();

        // Status-Updates
        Task<bool> UpdateStatusAsync(int id, string newStatus);
        Task<bool> MarkAsCompletedAsync(int id);
        Task<bool> MarkAsFailedAsync(int id, string errorMessage);
        Task<bool> RefundAsync(int id);

        // Existenzprüfungen
        Task<bool> ExistsAsync(int id);
        Task<bool> HasPendingPaymentAsync(int orderId);

        // Eager Loading mit Details
        Task<Payment?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Payment>> GetAllWithDetailsAsync();

        // Suche und Filterung
        Task<IEnumerable<Payment>> SearchAsync(string searchTerm);
        Task<IEnumerable<Payment>> GetFailedPaymentsAsync();
        Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
        Task<IEnumerable<Payment>> GetCompletedPaymentsAsync();

        // Bulk-Operationen
        Task<int> UpdateStatusBulkAsync(IEnumerable<int> ids, string newStatus);
        Task<IEnumerable<Payment>> AddRangeAsync(IEnumerable<Payment> payments);
    }
}
