using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;
using Xunit;

namespace Tests_BL_Backend.Repositories
{
    /// <summary>
    /// Unit tests for the OrderItemRepository class.
    /// </summary>
    public class OrderItemRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderItemRepository _repository;

        public OrderItemRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new OrderItemRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var productVariants = new List<ProductVariant>
            {
                new ProductVariant { Id = 1, ProductId = 1, SKU = "VAR-001", StockQuantity = 100, PriceAdjustment = 0m },
                new ProductVariant { Id = 2, ProductId = 1, SKU = "VAR-002", StockQuantity = 50, PriceAdjustment = 10m },
                new ProductVariant { Id = 100, ProductId = 10, SKU = "TEST-001", StockQuantity = 200, PriceAdjustment = 5m },
                new ProductVariant { Id = 200, ProductId = 20, SKU = "TEST-002", StockQuantity = 150, PriceAdjustment = 15m }
            };
            _context.ProductVariants.AddRange(productVariants);
            _context.SaveChanges();

            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1, OrderId = 1, ProductVariantId = 1, PriceAtPurchase = 29.99m, Quantity = 2 },
                new OrderItem { Id = 2, OrderId = 1, ProductVariantId = 2, PriceAtPurchase = 29.99m, Quantity = 1 },
                new OrderItem { Id = 3, OrderId = 2, ProductVariantId = 1, PriceAtPurchase = 19.99m, Quantity = 5 },
                new OrderItem { Id = 4, OrderId = 3, ProductVariantId = 100, PriceAtPurchase = 10.99m, Quantity = 1 },
                new OrderItem { Id = 5, OrderId = 3, ProductVariantId = 100, PriceAtPurchase = 20.99m, Quantity = 2 },
                new OrderItem { Id = 6, OrderId = 3, ProductVariantId = 100, PriceAtPurchase = 30.99m, Quantity = 3 },
                new OrderItem { Id = 7, OrderId = 4, ProductVariantId = 200, PriceAtPurchase = 40.99m, Quantity = 1 }
            };
            _context.OrderItems.AddRange(orderItems);
            _context.SaveChanges();
        }

        /// <summary>
        /// Tests that GetByIdAsync returns an OrderItem with ProductVariant included when a valid existing ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsOrderItemWithProductVariant()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.Quantity);
            Assert.Equal(29.99m, result.PriceAtPurchase);
            Assert.Equal(1, result.ProductVariantId);
            Assert.NotNull(result.ProductVariant);
            Assert.Equal("VAR-001", result.ProductVariant.SKU);
        }

        /// <summary>
        /// Tests that GetByIdAsync returns null when the provided ID does not exist in the database.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that GetByIdAsync returns null when provided with edge case ID values
        /// such as 0, negative numbers, int.MinValue, and int.MaxValue.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async Task GetByIdAsync_EdgeCaseIds_ReturnsNull(int id)
        {
            // Act
            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the constructor successfully creates an instance when provided with a valid ApplicationDbContext.
        /// </summary>
        [Fact]
        public void Constructor_ValidContext_CreatesInstance()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            // Act
            var repository = new OrderItemRepository(context);

            // Assert
            Assert.NotNull(repository);
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync returns all order items for a valid existing order ID.
        /// </summary>
        [Fact]
        public async Task GetByOrderIdAsync_ExistingOrderId_ReturnsAllOrderItems()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, oi => Assert.Equal(1, oi.OrderId));
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync returns an empty collection for a non-existent order ID.
        /// </summary>
        [Fact]
        public async Task GetByOrderIdAsync_NonExistingOrderId_ReturnsEmptyCollection()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync properly includes the ProductVariant navigation property.
        /// </summary>
        [Fact]
        public async Task GetByOrderIdAsync_ExistingOrderId_IncludesProductVariant()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, oi => Assert.NotNull(oi.ProductVariant));
            Assert.NotNull(result.First().ProductVariant);
            Assert.Equal("VAR-001", result.First().ProductVariant.SKU);
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync returns a single order item when only one exists for the order ID.
        /// </summary>
        [Fact]
        public async Task GetByOrderIdAsync_OrderIdWithSingleItem_ReturnsSingleItem()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var item = result.Single();
            Assert.Equal(2, item.OrderId);
            Assert.Equal(1, item.ProductVariantId);
            Assert.Equal(5, item.Quantity);
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync returns an empty collection for edge case order IDs.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async Task GetByOrderIdAsync_EdgeCaseOrderIds_ReturnsEmptyCollection(int orderId)
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync correctly filters order items to only return items matching the specified order ID.
        /// </summary>
        [Fact]
        public async Task GetByOrderIdAsync_MultipleOrdersExist_ReturnsOnlyMatchingOrderItems()
        {
            // Act
            var resultOrder1 = await _repository.GetByOrderIdAsync(1);
            var resultOrder2 = await _repository.GetByOrderIdAsync(2);

            // Assert
            Assert.Equal(2, resultOrder1.Count());
            Assert.All(resultOrder1, oi => Assert.Equal(1, oi.OrderId));

            Assert.Single(resultOrder2);
            Assert.All(resultOrder2, oi => Assert.Equal(2, oi.OrderId));
        }

        /// <summary>
        /// Tests that GetByOrderIdAsync returns items with correct property values matching the seed data.
        /// </summary>
        [Fact]
        public async Task GetByOrderIdAsync_ExistingOrderId_ReturnsItemsWithCorrectProperties()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(1);

            // Assert
            Assert.NotNull(result);
            var items = result.ToList();
            Assert.Equal(2, items.Count);

            var item1 = items.First(i => i.Id == 1);
            Assert.Equal(1, item1.ProductVariantId);
            Assert.Equal(2, item1.Quantity);
            Assert.Equal(29.99m, item1.PriceAtPurchase);

            var item2 = items.First(i => i.Id == 2);
            Assert.Equal(2, item2.ProductVariantId);
            Assert.Equal(1, item2.Quantity);
            Assert.Equal(29.99m, item2.PriceAtPurchase);
        }

        /// <summary>
        /// Tests that GetByProductVariantIdAsync returns all matching order items when multiple items exist with the specified ProductVariantId.
        /// </summary>
        [Fact]
        public async Task GetByProductVariantIdAsync_ExistingIdWithMultipleMatches_ReturnsAllMatching()
        {
            // Act
            var result = await _repository.GetByProductVariantIdAsync(100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.All(result, oi => Assert.Equal(100, oi.ProductVariantId));
            Assert.Contains(result, oi => oi.Id == 4);
            Assert.Contains(result, oi => oi.Id == 5);
            Assert.Contains(result, oi => oi.Id == 6);
        }

        /// <summary>
        /// Tests that GetByProductVariantIdAsync returns a single order item when only one item exists with the specified ProductVariantId.
        /// </summary>
        [Fact]
        public async Task GetByProductVariantIdAsync_ExistingIdWithSingleMatch_ReturnsSingleItem()
        {
            // Act
            var result = await _repository.GetByProductVariantIdAsync(200);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var orderItem = result.Single();
            Assert.Equal(7, orderItem.Id);
            Assert.Equal(200, orderItem.ProductVariantId);
            Assert.Equal(1, orderItem.Quantity);
            Assert.Equal(40.99m, orderItem.PriceAtPurchase);
        }

        /// <summary>
        /// Tests that GetByProductVariantIdAsync returns an empty collection when no order items exist with the specified ProductVariantId.
        /// </summary>
        [Fact]
        public async Task GetByProductVariantIdAsync_NonExistingId_ReturnsEmptyCollection()
        {
            // Act
            var result = await _repository.GetByProductVariantIdAsync(999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests that GetByProductVariantIdAsync returns an empty collection for edge case ProductVariantId values.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async Task GetByProductVariantIdAsync_EdgeCaseIds_ReturnsEmptyCollection(int productVariantId)
        {
            // Act
            var result = await _repository.GetByProductVariantIdAsync(productVariantId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}