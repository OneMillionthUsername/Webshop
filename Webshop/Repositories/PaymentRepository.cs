using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basis CRUD
        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatus(string status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPaymentMethod(string paymentMethod)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod == paymentMethod)
                .ToListAsync();
        }

        public async Task<Payment?> GetByTransactionId(int transactionId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId.ToString());
        }

        public async Task<Payment?> GetByDate(DateTime? date)
        {
            if (!date.HasValue)
                return null;

            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentDate.Date == date.Value.Date);
        }

        public async Task<(int Id, string PaymentMethod, int Count)> GetPaymentMethodAndCount(string paymentMethod)
        {
            var payments = await _context.Payments
                .Where(p => p.PaymentMethod == paymentMethod)
                .ToListAsync();

            var first = payments.FirstOrDefault();
            return (first?.Id ?? 0, paymentMethod, payments.Count);
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        // Bereichsabfragen
        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate.Date >= startDate.Date && p.PaymentDate.Date <= endDate.Date)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.Payments
                .Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                .OrderByDescending(p => p.Amount)
                .ToListAsync();
        }

        // Paginierung
        public async Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Payments.CountAsync();
            var payments = await _context.Payments
                .OrderByDescending(p => p.PaymentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (payments, totalCount);
        }

        public async Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPagedByStatusAsync(string status, int pageNumber, int pageSize)
        {
            var query = _context.Payments.Where(p => p.Status == status);
            var totalCount = await query.CountAsync();
            var payments = await query
                .OrderByDescending(p => p.PaymentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (payments, totalCount);
        }

        // Aggregationen und Statistiken
        public async Task<decimal> GetTotalAmountByStatusAsync(string status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .SumAsync(p => p.Amount);
        }

        public async Task<decimal> GetTotalAmountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .SumAsync(p => p.Amount);
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            return await _context.Payments
                .CountAsync(p => p.Status == status);
        }

        public async Task<decimal> GetAverageAmountAsync()
        {
            if (!await _context.Payments.AnyAsync())
                return 0;

            return await _context.Payments.AverageAsync(p => p.Amount);
        }

        public async Task<Dictionary<string, int>> GetPaymentMethodStatisticsAsync()
        {
            return await _context.Payments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new { PaymentMethod = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.PaymentMethod, x => x.Count);
        }

        // Status-Updates
        public async Task<bool> UpdateStatusAsync(int id, string newStatus)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                return false;

            payment.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsCompletedAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                return false;

            payment.Status = "Completed";
            payment.ErrorMessage = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsFailedAsync(int id, string errorMessage)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                return false;

            payment.Status = "Failed";
            payment.ErrorMessage = errorMessage;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RefundAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null || payment.Status != "Completed")
                return false;

            payment.Status = "Refunded";
            await _context.SaveChangesAsync();
            return true;
        }

        // Existenzprüfungen
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Payments.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> HasPendingPaymentAsync(int orderId)
        {
            return await _context.Payments
                .AnyAsync(p => p.OrderId == orderId && p.Status == "Pending");
        }

        // Eager Loading mit Details
        public async Task<Payment?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Details)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetAllWithDetailsAsync()
        {
            return await _context.Payments
                .Include(p => p.Details)
                .Include(p => p.Order)
                .ToListAsync();
        }

        // Suche und Filterung
        public async Task<IEnumerable<Payment>> SearchAsync(string searchTerm)
        {
            return await _context.Payments
                .Where(p => p.TransactionId.Contains(searchTerm) ||
                           p.OrderId.ToString().Contains(searchTerm) ||
                           p.PaymentMethod.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetFailedPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == "Failed")
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == "Pending")
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetCompletedPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == "Completed")
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        // Bulk-Operationen
        public async Task<int> UpdateStatusBulkAsync(IEnumerable<int> ids, string newStatus)
        {
            var payments = await _context.Payments
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            foreach (var payment in payments)
            {
                payment.Status = newStatus;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> AddRangeAsync(IEnumerable<Payment> payments)
        {
            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();
            return payments;
        }
    }
}
