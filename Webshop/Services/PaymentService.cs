using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Payment> ProcessStripePaymentAsync(string token, decimal amount, int orderId)
        {
            // TODO: Stripe SDK Integration
            // var stripeApiKey = _configuration["Stripe:SecretKey"];
            // var chargeService = new ChargeService();
            // var charge = await chargeService.CreateAsync(...);

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = "Stripe",
                Status = "Completed", // Simulation
                TransactionId = $"stripe_{Guid.NewGuid()}",
                PaymentDate = DateTime.UtcNow
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> ProcessPayPalPaymentAsync(string paymentId, decimal amount, int orderId)
        {
            // TODO: PayPal SDK Integration
            // var paypalClientId = _configuration["PayPal:ClientId"];
            // var paypalSecret = _configuration["PayPal:Secret"];

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = "PayPal",
                Status = "Completed", // Simulation
                TransactionId = paymentId,
                PaymentDate = DateTime.UtcNow
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> VerifyPaymentAsync(string paymentReference)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionId == paymentReference);

            return payment?.Status == "Completed";
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == paymentId)
                ?? throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
        }

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null || payment.Status == "Refunded")
                return false;

            // TODO: Stripe/PayPal Refund API aufrufen

            payment.Status = "Refunded";
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }
    }
}
