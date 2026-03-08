using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;

namespace Tests_BL_Backend.Repositories
{
    public class OrderRepositoryTests : IDisposable
	{
        private readonly ApplicationDbContext _context;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new OrderRepository(_context);

            SeedDatabase();
        }
        private void SeedDatabase()
        {
            var productVariants = new List<ProductVariant>
            {
                new ProductVariant { Id = 1, ProductId = 1, SKU = "VAR-001", StockQuantity = 100, PriceAdjustment = 0m },
                new ProductVariant { Id = 2, ProductId = 1, SKU = "VAR-002", StockQuantity = 50, PriceAdjustment = 10m }
            };
            _context.ProductVariants.AddRange(productVariants);
            _context.SaveChanges();

            var itemsCustomer1 = new List<OrderItem>
            {
                new OrderItem { Id = 1, OrderId = 1, ProductVariantId = 1, PriceAtPurchase = 99.99m, Quantity = 5},
                new OrderItem { Id = 2, OrderId = 2, ProductVariantId = 2, PriceAtPurchase = 349.89m, Quantity = 1 },
                new OrderItem { Id = 3, OrderId = 1, ProductVariantId = 2, PriceAtPurchase = 100.00m, Quantity = 3}
            };

            var itemsCustomer2 = new List<OrderItem>
            {
                new OrderItem { Id = 4, OrderId = 3, ProductVariantId = 1, PriceAtPurchase = 10m, Quantity = 5},
                new OrderItem { Id = 5, OrderId = 3, ProductVariantId = 2, PriceAtPurchase = 20m, Quantity = 3}
            };

            _context.OrderItems.AddRange(itemsCustomer1);
            _context.OrderItems.AddRange(itemsCustomer2);
            _context.SaveChanges();

            var ordersCustomer1 = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, Items=itemsCustomer1, OrderDate=DateTime.Parse("29.11.2025"), TotalAmount = 1149.84m},
                new Order { Id = 2, CustomerId = 1, Items=itemsCustomer2, OrderDate=DateTime.Parse("15.06.2023"), TotalAmount = 110m}
            };

            var ordersCustomer2 = new List<Order>
            {
                new Order { Id = 3, CustomerId = 2, Items=itemsCustomer1, OrderDate=DateTime.Parse("13.04.2022"), TotalAmount = 110m }
            };

            _context.Orders.AddRange(ordersCustomer1);
            _context.Orders.AddRange(ordersCustomer2);
            _context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Johnny", LastName = "Bravo", Address = "Beautystreet 29/5", City="Vienna", Email="johnny.bravo@gmail.com", PostalCode="1050", PhoneNumber="+436981654875", RegistrationDate=DateTime.Parse("12.05.2020"), Orders = ordersCustomer1},
                new Customer { Id = 2, FirstName = "Bonda", LastName = "Sukumba", Address = "Pupuleto 3", City="Imbudu", Email="paransa@kongo.kg", PostalCode="8745", PhoneNumber="+64223461262", RegistrationDate=DateTime.Parse("19.03.2017"), Orders = ordersCustomer2}
            };
            _context.Customers.AddRange(customers);
            _context.SaveChanges();
        }
        [Fact]
        public async Task GetAllAsync_ReturnAllOrders()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.True(result.Count() > 0);
        }
        [Fact]
        public async Task GetByIdAsync_ReturnOrder()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1149.84m, result.TotalAmount);
            Assert.Equal(1, result.CustomerId);
            Assert.Equal(3, result.Items.Count);
        }
        [Fact]
        public async Task GetByIdAsync_ReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetByCustomerIdAsync_ReturnOrders()
        {
            // Act
            var result1 = await _repository.GetByCustomerIdAsync(1);
            var result2 = await _repository.GetByCustomerIdAsync(2);
            // Assert - für Customer 1
            Assert.NotEmpty(result1);
            Assert.Equal(2, result1.Count());

            // Assert - für Customer 2
            Assert.NotEmpty(result2);
            Assert.Single(result2);
        }
        [Fact]
        public async Task GetByCustomerIdAsync_ReturnEmpty()
        {
            // Act
            var result = await _repository.GetByCustomerIdAsync(999);
            // Assert - für Customer 1
            Assert.Empty(result);
        }
        [Fact]
        public async Task ExistsAsync_ReturnTrue()
        {
            // Act
            var result = await _repository.ExistsAsync(3);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ExistsAsync_ReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task DeleteAsync_ReturnFalse()
        {
            // Act
            await _repository.DeleteAsync(3);
            var result = await _repository.GetByIdAsync(3);
            var remaining = await _repository.GetAllAsync();

            // Assert
            Assert.Null(result);
            Assert.Equal(2, remaining.Count());
        }
        [Fact]
        public async Task DeleteAsync_ReturnTrue()
        {
            await _repository.DeleteAsync(999);
            var result = await _repository.GetByIdAsync(999);
            var remaining = await _repository.GetAllAsync();
            Assert.Null(result);
            Assert.Equal(3, remaining.Count());
        }
        [Fact]
        public async Task AddAsync_ReturnOrder()
        {
            // Arrange
            var newOrderItem = new OrderItem 
            { 
                ProductVariantId = 1, 
                PriceAtPurchase = 199.99m, 
                Quantity = 2 
            };

            Order order = new Order
            {
                Id = 4,
                CustomerId = 1,
                Items = new List<OrderItem> { newOrderItem },
                Payments = new List<Payment>(),
                OrderDate = DateTime.Now,
                TotalAmount = 399.98m
            };

            // Act
            await _repository.AddAsync(order);

            var result = await _repository.GetByIdAsync(4);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.TotalAmount, result.TotalAmount);
            Assert.Single(result.Items);
            Assert.NotNull(result.Items.First().ProductVariant);
            Assert.Equal(1, result.Items.First().ProductVariantId);
        }
        [Fact]
        public async Task AddAsync_ThrowsEx()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null));
        }
        [Fact]
        public async Task UpdateAsync_ReturnOrder()
        {
            // Arrange
            var order = await _repository.GetByIdAsync(1);

            // Act
            order.OrderDate = DateTime.Now;
            order.TotalAmount = 44.99m;
            await _repository.UpdateAsync(order);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.TotalAmount, result.TotalAmount);
            Assert.Equal(order.OrderDate, result.OrderDate);
        }
        [Fact]
        public async Task UpdateAsync_ThrowsEx()
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null));
        }
		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}
}
