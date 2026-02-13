using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;
using Xunit;

namespace Tests_BL_Backend.Repositories
{
    public class PaymentRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly PaymentRepository _repository;

        public PaymentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new PaymentRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Max", LastName = "Mustermann", Email = "max@test.com", Address = "Teststraße 1", City = "Wien", PostalCode = "1010", PhoneNumber = "+43123456789", RegistrationDate = DateTime.Parse("2020-01-01") },
                new Customer { Id = 2, FirstName = "Anna", LastName = "Schmidt", Email = "anna@test.com", Address = "Hauptstraße 2", City = "Graz", PostalCode = "8010", PhoneNumber = "+43987654321", RegistrationDate = DateTime.Parse("2021-01-01") },
                new Customer { Id = 3, FirstName = "Test", LastName = "User", Email = "test@test.com", Address = "Testweg 3", City = "Linz", PostalCode = "4020", PhoneNumber = "+43111111111", RegistrationDate = DateTime.Parse("2022-01-01") }
            };
            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Parse("2024-01-15"), TotalAmount = 99.99m },
                new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.Parse("2024-02-10"), TotalAmount = 299.99m },
                new Order { Id = 3, CustomerId = 2, OrderDate = DateTime.Parse("2024-03-05"), TotalAmount = 149.99m },
                new Order { Id = 4, CustomerId = 2, OrderDate = DateTime.Parse("2024-06-20"), TotalAmount = 500.00m },
                new Order { Id = 5, CustomerId = 3, OrderDate = DateTime.Parse("2024-07-01"), TotalAmount = 1000.00m },
                new Order { Id = 6, CustomerId = 3, OrderDate = DateTime.Parse("2023-12-31"), TotalAmount = 75.50m },
                new Order { Id = 7, CustomerId = 1, OrderDate = DateTime.Parse("2022-01-01"), TotalAmount = 200.00m }
            };
            _context.Orders.AddRange(orders);
            _context.SaveChanges();

            var payments = new List<Payment>
            {
                // Standard successful payments
                new Payment { Id = 1, OrderId = 1, Amount = 99.99m, PaymentMethod = "CreditCard", Status = "Completed", TransactionId = "TXN001", PaymentDate = DateTime.Parse("2024-01-15") },
                new Payment { Id = 2, OrderId = 2, Amount = 299.99m, PaymentMethod = "PayPal", Status = "Pending", TransactionId = "TXN002", PaymentDate = DateTime.Parse("2024-02-10") },
                new Payment { Id = 3, OrderId = 3, Amount = 149.99m, PaymentMethod = "CreditCard", Status = "Failed", TransactionId = "TXN003", PaymentDate = DateTime.Parse("2024-03-05"), ErrorMessage = "Card declined" },
                new Payment { Id = 4, OrderId = 1, Amount = 50.00m, PaymentMethod = "Stripe", Status = "Completed", TransactionId = "TXN004", PaymentDate = DateTime.Parse("2024-01-20") },

                // Edge cases
                new Payment { Id = 5, OrderId = 4, Amount = 0.01m, PaymentMethod = "CreditCard", Status = "Completed", TransactionId = "TXN005", PaymentDate = DateTime.Parse("2024-06-20") }, // Minimum amount
                new Payment { Id = 6, OrderId = 5, Amount = 99999.99m, PaymentMethod = "PayPal", Status = "Completed", TransactionId = "TXN006", PaymentDate = DateTime.Parse("2024-07-01") }, // Very large amount
                new Payment { Id = 7, OrderId = 6, Amount = 75.50m, PaymentMethod = "", Status = "Pending", TransactionId = "TXN007", PaymentDate = DateTime.Parse("2023-12-31") }, // Empty payment method
                new Payment { Id = 8, OrderId = 7, Amount = 200.00m, PaymentMethod = "Unknown@Method!", Status = "Refunded", TransactionId = "TXN008", PaymentDate = DateTime.Parse("2022-01-01") }, // Special chars in method

                // Same day payments
                new Payment { Id = 9, OrderId = 1, Amount = 10.00m, PaymentMethod = "CreditCard", Status = "Completed", TransactionId = "TXN009", PaymentDate = DateTime.Parse("2024-01-15 10:30:00") },
                new Payment { Id = 10, OrderId = 2, Amount = 20.00m, PaymentMethod = "CreditCard", Status = "Completed", TransactionId = "TXN010", PaymentDate = DateTime.Parse("2024-01-15 14:45:00") },

                // Failed with various error messages
                new Payment { Id = 11, OrderId = 3, Amount = 100.00m, PaymentMethod = "PayPal", Status = "Failed", TransactionId = "TXN011", PaymentDate = DateTime.Parse("2024-03-10"), ErrorMessage = "Insufficient funds" },
                new Payment { Id = 12, OrderId = 4, Amount = 50.00m, PaymentMethod = "Stripe", Status = "Failed", TransactionId = "TXN012", PaymentDate = DateTime.Parse("2024-03-15"), ErrorMessage = "" }, // Empty error
                new Payment { Id = 13, OrderId = 5, Amount = 150.00m, PaymentMethod = "CreditCard", Status = "Failed", TransactionId = "TXN013", PaymentDate = DateTime.Parse("2024-03-20"), ErrorMessage = "Error with\nnewlines\nand\ttabs" },

                // Multiple payments per order
                new Payment { Id = 14, OrderId = 1, Amount = 25.00m, PaymentMethod = "PayPal", Status = "Pending", TransactionId = "TXN014", PaymentDate = DateTime.Parse("2024-01-16") },
                new Payment { Id = 15, OrderId = 1, Amount = 15.00m, PaymentMethod = "Stripe", Status = "Pending", TransactionId = "TXN015", PaymentDate = DateTime.Parse("2024-01-17") }
            };
            _context.Payments.AddRange(payments);
            _context.SaveChanges();

            var paymentDetails = new List<PaymentDetail>
            {
                new CreditCardPaymentDetail { Id = 1, PaymentId = 1, CardBrand = "Visa", Last4 = "1234", ExpiryMonth = 12, ExpiryYear = 2025, Token = "tok_123" },
                new PayPalPaymentDetail { Id = 2, PaymentId = 2, PayerEmail = "max@test.com", ProviderPaymentId = "PP123" },
                new CreditCardPaymentDetail { Id = 3, PaymentId = 3, CardBrand = "Mastercard", Last4 = "5678", ExpiryMonth = 6, ExpiryYear = 2024, Token = "tok_456" },
                new CreditCardPaymentDetail { Id = 4, PaymentId = 5, CardBrand = "AmEx", Last4 = "0001", ExpiryMonth = 1, ExpiryYear = 2030, Token = "tok_edge" },
                new EpsPaymentDetail { Id = 5, PaymentId = 8, BankName = "Test Bank", Reference = "REF123", ProviderPaymentId = "EPS456" },
                new CreditCardPaymentDetail { Id = 6, PaymentId = 13, CardBrand = "Visa", Last4 = "9999", ExpiryMonth = 12, ExpiryYear = 2099, Token = "tok_future" }
            };
            _context.PaymentDetails.AddRange(paymentDetails);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region Basis CRUD Tests

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsPayment()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(99.99m, result.Amount);
            Assert.Equal("CreditCard", result.PaymentMethod);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_EdgeCaseIds_ReturnsNull()
        {
            // Act & Assert
            Assert.Null(await _repository.GetByIdAsync(0));
            Assert.Null(await _repository.GetByIdAsync(-1));
            Assert.Null(await _repository.GetByIdAsync(int.MinValue));
            Assert.Null(await _repository.GetByIdAsync(int.MaxValue));
        }

        [Fact]
        public async Task GetByOrderIdAsync_ExistingOrderId_ReturnsPayment()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.OrderId);
            Assert.Equal("PayPal", result.PaymentMethod);
        }

        [Fact]
        public async Task GetByOrderIdAsync_OrderWithMultiplePayments_ReturnsFirstPayment()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
        }

        [Fact]
        public async Task GetByOrderIdAsync_NonExistingOrderId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPayments()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.Count());
        }

        [Fact]
        public async Task GetByStatus_ReturnsFilteredPayments()
        {
            // Act
            var result = await _repository.GetByStatus("Completed");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.All(result, p => Assert.Equal("Completed", p.Status));
        }

        [Fact]
        public async Task GetByStatus_NonExistingStatus_ReturnsEmpty()
        {
            // Act
            var result = await _repository.GetByStatus("NonExistingStatus");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByStatus_CaseSensitive_ReturnsCorrectResults()
        {
            // Act
            var lowercase = await _repository.GetByStatus("completed");
            var uppercase = await _repository.GetByStatus("COMPLETED");

            // Assert
            Assert.Empty(lowercase);
            Assert.Empty(uppercase);
        }

        [Fact]
        public async Task GetByPaymentMethod_ReturnsFilteredPayments()
        {
            // Act
            var result = await _repository.GetByPaymentMethod("CreditCard");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.All(result, p => Assert.Equal("CreditCard", p.PaymentMethod));
        }

        [Fact]
        public async Task GetByPaymentMethod_EmptyString_ReturnsPaymentsWithEmptyMethod()
        {
            // Act
            var result = await _repository.GetByPaymentMethod("");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(7, result.First().Id);
        }

        [Fact]
        public async Task GetByPaymentMethod_SpecialCharacters_ReturnsCorrectPayment()
        {
            // Act
            var result = await _repository.GetByPaymentMethod("Unknown@Method!");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(8, result.First().Id);
        }

        [Fact]
        public async Task AddAsync_AddsPaymentToDatabase()
        {
            // Arrange
            var payment = new Payment
            {
                Amount = 75m,
                PaymentMethod = "Stripe",
                Status = "Pending",
                TransactionId = "TXN789",
                OrderId = 1
            };

            // Act
            var result = await _repository.AddAsync(payment);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal(75m, result.Amount);

            var dbPayment = await _context.Payments.FindAsync(result.Id);
            Assert.NotNull(dbPayment);
            Assert.Equal("Stripe", dbPayment.PaymentMethod);
        }

        [Fact]
        public async Task AddAsync_MinimumAmount_AddsSuccessfully()
        {
            // Arrange
            var payment = new Payment
            {
                Amount = 0.01m,
                PaymentMethod = "CreditCard",
                Status = "Completed",
                TransactionId = "TXN_MIN",
                OrderId = 1
            };

            // Act
            var result = await _repository.AddAsync(payment);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal(0.01m, result.Amount);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPaymentInDatabase()
        {
            // Arrange
            var payment = await _context.Payments.FindAsync(2);
            Assert.NotNull(payment);
            payment.Status = "Completed";
            payment.Amount = 320m;

            // Act
            var result = await _repository.UpdateAsync(payment);

            // Assert
            Assert.Equal("Completed", result.Status);
            Assert.Equal(320m, result.Amount);

            var dbPayment = await _context.Payments.FindAsync(2);
            Assert.Equal("Completed", dbPayment!.Status);
            Assert.Equal(320m, dbPayment.Amount);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPaymentFromDatabase()
        {
            // Act
            var result = await _repository.DeleteAsync(4);

            // Assert
            Assert.True(result);
            var dbPayment = await _context.Payments.FindAsync(4);
            Assert.Null(dbPayment);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_EdgeCaseIds_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(await _repository.DeleteAsync(0));
            Assert.False(await _repository.DeleteAsync(-1));
            Assert.False(await _repository.DeleteAsync(int.MinValue));
        }

        #endregion

        #region Bereichsabfragen Tests

        [Fact]
        public async Task GetByDateRangeAsync_ReturnsPaymentsInRange()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            // Act
            var result = await _repository.GetByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.All(result, p => Assert.True(p.PaymentDate >= startDate && p.PaymentDate <= endDate));
        }

        [Fact]
        public async Task GetByDateRangeAsync_SameDay_ReturnsAllPaymentsOnThatDay()
        {
            // Arrange
            var date = new DateTime(2024, 1, 15);

            // Act
            var result = await _repository.GetByDateRangeAsync(date, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByDateRangeAsync_NoPaymentsInRange_ReturnsEmpty()
        {
            // Arrange
            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 12, 31);

            // Act
            var result = await _repository.GetByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByDateRangeAsync_InvertedRange_ReturnsEmpty()
        {
            // Arrange
            var startDate = new DateTime(2024, 12, 31);
            var endDate = new DateTime(2024, 1, 1);

            // Act
            var result = await _repository.GetByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByAmountRangeAsync_ReturnsPaymentsInRange()
        {
            // Act
            var result = await _repository.GetByAmountRangeAsync(100m, 200m);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.All(result, p => Assert.True(p.Amount >= 100m && p.Amount <= 200m));
        }

        [Fact]
        public async Task GetByAmountRangeAsync_MinimumAmount_ReturnsCorrectPayment()
        {
            // Act
            var result = await _repository.GetByAmountRangeAsync(0.01m, 0.01m);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(5, result.First().Id);
        }

        [Fact]
        public async Task GetByAmountRangeAsync_VeryLargeAmount_ReturnsCorrectPayment()
        {
            // Act
            var result = await _repository.GetByAmountRangeAsync(50000m, 100000m);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(6, result.First().Id);
        }

        [Fact]
        public async Task GetByAmountRangeAsync_NoPaymentsInRange_ReturnsEmpty()
        {
            // Act
            var result = await _repository.GetByAmountRangeAsync(200000m, 300000m);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region Paginierung Tests

        [Fact]
        public async Task GetPagedAsync_ReturnsCorrectPage()
        {
            // Act
            var (pagedPayments, totalCount) = await _repository.GetPagedAsync(1, 5);

            // Assert
            Assert.NotNull(pagedPayments);
            Assert.Equal(5, pagedPayments.Count());
            Assert.Equal(15, totalCount);
        }

        [Fact]
        public async Task GetPagedAsync_SecondPage_ReturnsCorrectPayments()
        {
            // Act
            var (pagedPayments, totalCount) = await _repository.GetPagedAsync(2, 5);

            // Assert
            Assert.NotNull(pagedPayments);
            Assert.Equal(5, pagedPayments.Count());
            Assert.Equal(15, totalCount);
        }

        [Fact]
        public async Task GetPagedAsync_LastPage_ReturnsRemainingPayments()
        {
            // Act
            var (pagedPayments, totalCount) = await _repository.GetPagedAsync(3, 7);

            // Assert
            Assert.NotNull(pagedPayments);
            Assert.Single(pagedPayments);
            Assert.Equal(15, totalCount);
        }

        [Fact]
        public async Task GetPagedAsync_PageBeyondEnd_ReturnsEmpty()
        {
            // Act
            var (pagedPayments, totalCount) = await _repository.GetPagedAsync(10, 10);

            // Assert
            Assert.NotNull(pagedPayments);
            Assert.Empty(pagedPayments);
            Assert.Equal(15, totalCount);
        }

        [Fact]
        public async Task GetPagedAsync_OrderedByDateDescending_ReturnsNewestFirst()
        {
            // Act
            var (pagedPayments, _) = await _repository.GetPagedAsync(1, 5);

            // Assert
            var paymentsList = pagedPayments.ToList();
            for (int i = 0; i < paymentsList.Count - 1; i++)
            {
                Assert.True(paymentsList[i].PaymentDate >= paymentsList[i + 1].PaymentDate);
            }
        }

        [Fact]
        public async Task GetPagedByStatusAsync_ReturnsCorrectPageWithFilter()
        {
            // Act
            var (pagedPayments, totalCount) = await _repository.GetPagedByStatusAsync("Completed", 1, 3);

            // Assert
            Assert.NotNull(pagedPayments);
            Assert.Equal(3, pagedPayments.Count());
            Assert.Equal(6, totalCount);
            Assert.All(pagedPayments, p => Assert.Equal("Completed", p.Status));
        }

        [Fact]
        public async Task GetPagedByStatusAsync_Failed_ReturnsOnlyFailedPayments()
        {
            // Act
            var (pagedPayments, totalCount) = await _repository.GetPagedByStatusAsync("Failed", 1, 10);

            // Assert
            Assert.NotNull(pagedPayments);
            Assert.Equal(4, totalCount);
            Assert.All(pagedPayments, p => Assert.Equal("Failed", p.Status));
        }

        #endregion

        #region Aggregationen Tests

        [Fact]
        public async Task GetTotalAmountByStatusAsync_ReturnsCorrectSum()
        {
            // Act
            var result = await _repository.GetTotalAmountByStatusAsync("Completed");

            // Assert
            Assert.Equal(100179.99m, result);
        }

        [Fact]
        public async Task GetTotalAmountByStatusAsync_Failed_ReturnsCorrectSum()
        {
            // Act
            var result = await _repository.GetTotalAmountByStatusAsync("Failed");

            // Assert
            Assert.Equal(449.99m, result);
        }

        [Fact]
        public async Task GetTotalAmountByStatusAsync_NonExistingStatus_ReturnsZero()
        {
            // Act
            var result = await _repository.GetTotalAmountByStatusAsync("NonExisting");

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public async Task GetTotalAmountByDateRangeAsync_ReturnsCorrectSum()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            // Act
            var result = await _repository.GetTotalAmountByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.Equal(219.99m, result);
        }

        [Fact]
        public async Task GetTotalAmountByDateRangeAsync_EmptyRange_ReturnsZero()
        {
            // Arrange
            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 12, 31);

            // Act
            var result = await _repository.GetTotalAmountByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public async Task GetCountByStatusAsync_ReturnsCorrectCount()
        {
            // Act
            var result = await _repository.GetCountByStatusAsync("Completed");

            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task GetCountByStatusAsync_Pending_ReturnsCorrectCount()
        {
            // Act
            var result = await _repository.GetCountByStatusAsync("Pending");

            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GetCountByStatusAsync_NonExistingStatus_ReturnsZero()
        {
            // Act
            var result = await _repository.GetCountByStatusAsync("DoesNotExist");

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetAverageAmountAsync_ReturnsCorrectAverage()
        {
            // Act
            var result = await _repository.GetAverageAmountAsync();

            // Assert
            Assert.True(result > 0);
            Assert.Equal(6749.70m, Math.Round(result, 2));
        }

        [Fact]
        public async Task GetAverageAmountAsync_NoPayments_ReturnsZero()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var emptyContext = new ApplicationDbContext(options);
            var emptyRepository = new PaymentRepository(emptyContext);

            // Act
            var result = await emptyRepository.GetAverageAmountAsync();

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public async Task GetPaymentMethodStatisticsAsync_ReturnsCorrectDictionary()
        {
            // Act
            var result = await _repository.GetPaymentMethodStatisticsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(6, result["CreditCard"]);
            Assert.Equal(4, result["PayPal"]);
            Assert.Equal(3, result["Stripe"]);
            Assert.Equal(1, result[""]);
            Assert.Equal(1, result["Unknown@Method!"]);
        }

        [Fact]
        public async Task GetPaymentMethodStatisticsAsync_EmptyDatabase_ReturnsEmptyDictionary()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            using var emptyContext = new ApplicationDbContext(options);
            var emptyRepository = new PaymentRepository(emptyContext);

            // Act
            var result = await emptyRepository.GetPaymentMethodStatisticsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region Status-Updates Tests

        [Fact]
        public async Task UpdateStatusAsync_UpdatesStatusSuccessfully()
        {
            // Act
            var result = await _repository.UpdateStatusAsync(2, "Completed");

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(2);
            Assert.Equal("Completed", updatedPayment!.Status);
        }

        [Fact]
        public async Task UpdateStatusAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.UpdateStatusAsync(999, "Completed");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateStatusAsync_EdgeCaseIds_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(await _repository.UpdateStatusAsync(0, "Completed"));
            Assert.False(await _repository.UpdateStatusAsync(-1, "Completed"));
            Assert.False(await _repository.UpdateStatusAsync(int.MinValue, "Completed"));
        }

        [Fact]
        public async Task MarkAsCompletedAsync_MarksPaymentAsCompleted()
        {
            // Act
            var result = await _repository.MarkAsCompletedAsync(3);

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(3);
            Assert.Equal("Completed", updatedPayment!.Status);
            Assert.Null(updatedPayment.ErrorMessage);
        }

        [Fact]
        public async Task MarkAsCompletedAsync_ClearsErrorMessage()
        {
            // Arrange
            var payment = await _context.Payments.FindAsync(11);
            Assert.NotNull(payment);
            Assert.NotNull(payment.ErrorMessage);

            // Act
            var result = await _repository.MarkAsCompletedAsync(11);

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(11);
            Assert.Null(updatedPayment!.ErrorMessage);
        }

        [Fact]
        public async Task MarkAsCompletedAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.MarkAsCompletedAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task MarkAsCompletedAsync_AlreadyCompleted_RemainsCompleted()
        {
            // Act
            var result = await _repository.MarkAsCompletedAsync(1);

            // Assert
            Assert.True(result);
            var payment = await _context.Payments.FindAsync(1);
            Assert.Equal("Completed", payment!.Status);
        }

        [Fact]
        public async Task MarkAsFailedAsync_MarksPaymentAsFailed()
        {
            // Act
            var result = await _repository.MarkAsFailedAsync(2, "Payment declined");

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(2);
            Assert.Equal("Failed", updatedPayment!.Status);
            Assert.Equal("Payment declined", updatedPayment.ErrorMessage);
        }

        [Fact]
        public async Task MarkAsFailedAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.MarkAsFailedAsync(999, "Error");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task MarkAsFailedAsync_NullErrorMessage_StoresNull()
        {
            // Act
            var result = await _repository.MarkAsFailedAsync(2, null!);

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(2);
            Assert.Equal("Failed", updatedPayment!.Status);
            Assert.Null(updatedPayment.ErrorMessage);
        }

        [Fact]
        public async Task MarkAsFailedAsync_EmptyErrorMessage_StoresEmpty()
        {
            // Act
            var result = await _repository.MarkAsFailedAsync(2, "");

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(2);
            Assert.Equal("Failed", updatedPayment!.Status);
            Assert.Equal("", updatedPayment.ErrorMessage);
        }

        [Fact]
        public async Task MarkAsFailedAsync_ErrorMessageWithSpecialChars_StoresCorrectly()
        {
            // Arrange
            var errorMessage = "Error with\nnewlines\tand\ttabs!@#$%";

            // Act
            var result = await _repository.MarkAsFailedAsync(2, errorMessage);

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(2);
            Assert.Equal(errorMessage, updatedPayment!.ErrorMessage);
        }

        [Fact]
        public async Task MarkAsFailedAsync_UpdatesAlreadyFailedPayment()
        {
            // Arrange
            var payment = await _context.Payments.FindAsync(3);
            Assert.Equal("Failed", payment!.Status);
            Assert.Equal("Card declined", payment.ErrorMessage);

            // Act
            var result = await _repository.MarkAsFailedAsync(3, "New error message");

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(3);
            Assert.Equal("Failed", updatedPayment!.Status);
            Assert.Equal("New error message", updatedPayment.ErrorMessage);
        }

        [Fact]
        public async Task RefundAsync_CompletedPayment_MarksAsRefunded()
        {
            // Act
            var result = await _repository.RefundAsync(1);

            // Assert
            Assert.True(result);
            var updatedPayment = await _context.Payments.FindAsync(1);
            Assert.Equal("Refunded", updatedPayment!.Status);
        }

        [Fact]
        public async Task RefundAsync_NonCompletedPayment_ReturnsFalse()
        {
            // Act
            var result = await _repository.RefundAsync(2);

            // Assert
            Assert.False(result);
            var payment = await _context.Payments.FindAsync(2);
            Assert.Equal("Pending", payment!.Status);
        }

        [Fact]
        public async Task RefundAsync_FailedPayment_ReturnsFalse()
        {
            // Act
            var result = await _repository.RefundAsync(3);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RefundAsync_AlreadyRefunded_ReturnsFalse()
        {
            // Act
            var result = await _repository.RefundAsync(8);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RefundAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.RefundAsync(999);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Existenzprüfungen Tests

        [Fact]
        public async Task ExistsAsync_ExistingId_ReturnsTrue()
        {
            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_EdgeCaseIds_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(await _repository.ExistsAsync(0));
            Assert.False(await _repository.ExistsAsync(-1));
            Assert.False(await _repository.ExistsAsync(int.MinValue));
            Assert.False(await _repository.ExistsAsync(int.MaxValue));
        }

        [Fact]
        public async Task HasPendingPaymentAsync_ExistingPendingPayment_ReturnsTrue()
        {
            // Act
            var result = await _repository.HasPendingPaymentAsync(2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task HasPendingPaymentAsync_NoPendingPayment_ReturnsFalse()
        {
            // Act
            var result = await _repository.HasPendingPaymentAsync(3);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task HasPendingPaymentAsync_OrderWithMultiplePendingPayments_ReturnsTrue()
        {
            // Act
            var result = await _repository.HasPendingPaymentAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task HasPendingPaymentAsync_NonExistingOrder_ReturnsFalse()
        {
            // Act
            var result = await _repository.HasPendingPaymentAsync(999);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Eager Loading Tests

        [Fact]
        public async Task GetByIdWithDetailsAsync_LoadsPaymentDetails()
        {
            // Act
            var result = await _repository.GetByIdWithDetailsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Details);
            Assert.IsType<CreditCardPaymentDetail>(result.Details);
            Assert.NotNull(result.Order);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_LoadsEpsPaymentDetail()
        {
            // Act
            var result = await _repository.GetByIdWithDetailsAsync(8);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Details);
            Assert.IsType<EpsPaymentDetail>(result.Details);
            var epsDetail = result.Details as EpsPaymentDetail;
            Assert.Equal("Test Bank", epsDetail!.BankName);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_PaymentWithoutDetails_LoadsPaymentOnly()
        {
            // Act
            var result = await _repository.GetByIdWithDetailsAsync(4);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Details);
            Assert.NotNull(result.Order);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdWithDetailsAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllWithDetailsAsync_LoadsAllPaymentsWithDetails()
        {
            // Act
            var result = await _repository.GetAllWithDetailsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.Count());
        }

        [Fact]
        public async Task GetAllWithDetailsAsync_IncludesPaymentsWithoutDetails()
        {
            // Act
            var result = await _repository.GetAllWithDetailsAsync();
            var paymentsWithoutDetails = result.Where(p => p.Details == null);

            // Assert
            Assert.NotEmpty(paymentsWithoutDetails);
        }

        #endregion

        #region Suche und Filterung Tests

        [Fact]
        public async Task SearchAsync_FindsPaymentsByTransactionId()
        {
            // Act
            var result = await _repository.SearchAsync("TXN00");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Count());
        }

        [Fact]
        public async Task SearchAsync_FindsPaymentsByOrderId()
        {
            // Act
            var result = await _repository.SearchAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count() >= 5);
        }

        [Fact]
        public async Task SearchAsync_FindsPaymentsByPaymentMethod()
        {
            // Act
            var result = await _repository.SearchAsync("Credit");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task SearchAsync_NoMatch_ReturnsEmpty()
        {
            // Act
            var result = await _repository.SearchAsync("DoesNotExist12345");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchAsync_EmptyString_ReturnsAllPayments()
        {
            // Act
            var result = await _repository.SearchAsync("");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.Count());
        }

        [Fact]
        public async Task GetFailedPaymentsAsync_ReturnsOnlyFailedPayments()
        {
            // Act
            var result = await _repository.GetFailedPaymentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.All(result, p => Assert.Equal("Failed", p.Status));
        }

        [Fact]
        public async Task GetFailedPaymentsAsync_OrderedByDateDescending()
        {
            // Act
            var result = await _repository.GetFailedPaymentsAsync();
            var resultList = result.ToList();

            // Assert
            for (int i = 0; i < resultList.Count - 1; i++)
            {
                Assert.True(resultList[i].PaymentDate >= resultList[i + 1].PaymentDate);
            }
        }

        [Fact]
        public async Task GetPendingPaymentsAsync_ReturnsOnlyPendingPayments()
        {
            // Act
            var result = await _repository.GetPendingPaymentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.All(result, p => Assert.Equal("Pending", p.Status));
        }

        [Fact]
        public async Task GetCompletedPaymentsAsync_ReturnsOnlyCompletedPayments()
        {
            // Act
            var result = await _repository.GetCompletedPaymentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
            Assert.All(result, p => Assert.Equal("Completed", p.Status));
        }

        [Fact]
        public async Task GetCompletedPaymentsAsync_OrderedByDateDescending()
        {
            // Act
            var result = await _repository.GetCompletedPaymentsAsync();
            var resultList = result.ToList();

            // Assert
            for (int i = 0; i < resultList.Count - 1; i++)
            {
                Assert.True(resultList[i].PaymentDate >= resultList[i + 1].PaymentDate);
            }
        }

        #endregion

        #region Bulk-Operationen Tests

        [Fact]
        public async Task UpdateStatusBulkAsync_UpdatesMultiplePayments()
        {
            // Arrange
            var ids = new[] { 2, 3, 11 };

            // Act
            var affectedRows = await _repository.UpdateStatusBulkAsync(ids, "Completed");

            // Assert
            Assert.True(affectedRows > 0);
            var updatedPayments = await _context.Payments.Where(p => ids.Contains(p.Id)).ToListAsync();
            Assert.All(updatedPayments, p => Assert.Equal("Completed", p.Status));
        }

        [Fact]
        public async Task UpdateStatusBulkAsync_EmptyIdList_ReturnsZero()
        {
            // Arrange
            var ids = Array.Empty<int>();

            // Act
            var affectedRows = await _repository.UpdateStatusBulkAsync(ids, "Completed");

            // Assert
            Assert.Equal(0, affectedRows);
        }

        [Fact]
        public async Task UpdateStatusBulkAsync_NonExistingIds_ReturnsZero()
        {
            // Arrange
            var ids = new[] { 999, 1000, 1001 };

            // Act
            var affectedRows = await _repository.UpdateStatusBulkAsync(ids, "Completed");

            // Assert
            Assert.Equal(0, affectedRows);
        }

        [Fact]
        public async Task UpdateStatusBulkAsync_MixedExistingAndNonExisting_UpdatesOnlyExisting()
        {
            // Arrange
            var ids = new[] { 2, 999, 3 };

            // Act
            var affectedRows = await _repository.UpdateStatusBulkAsync(ids, "Completed");

            // Assert
            Assert.True(affectedRows > 0);
            var payment2 = await _context.Payments.FindAsync(2);
            var payment3 = await _context.Payments.FindAsync(3);
            Assert.Equal("Completed", payment2!.Status);
            Assert.Equal("Completed", payment3!.Status);
        }

        [Fact]
        public async Task AddRangeAsync_AddsMultiplePayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new() { Amount = 100m, PaymentMethod = "CreditCard", Status = "Completed", TransactionId = "TXN100", OrderId = 1 },
                new() { Amount = 200m, PaymentMethod = "PayPal", Status = "Pending", TransactionId = "TXN200", OrderId = 2 },
                new() { Amount = 50m, PaymentMethod = "Stripe", Status = "Failed", TransactionId = "TXN300", OrderId = 3 }
            };

            // Act
            var result = await _repository.AddRangeAsync(payments);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            var dbPayments = await _context.Payments.ToListAsync();
            Assert.Equal(18, dbPayments.Count);
        }

        [Fact]
        public async Task AddRangeAsync_EmptyList_AddsNothing()
        {
            // Arrange
            var payments = new List<Payment>();

            // Act
            var result = await _repository.AddRangeAsync(payments);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            var dbPayments = await _context.Payments.ToListAsync();
            Assert.Equal(15, dbPayments.Count);
        }

        [Fact]
        public async Task AddRangeAsync_AssignsNewIds()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new() { Amount = 100m, PaymentMethod = "CreditCard", Status = "Completed", TransactionId = "TXN999", OrderId = 1 },
                new() { Amount = 200m, PaymentMethod = "PayPal", Status = "Pending", TransactionId = "TXN998", OrderId = 2 }
            };

            // Act
            var result = await _repository.AddRangeAsync(payments);

            // Assert
            var resultList = result.ToList();
            Assert.All(resultList, p => Assert.NotEqual(0, p.Id));
            Assert.NotEqual(resultList[0].Id, resultList[1].Id);
        }

        #endregion
    }
}