using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;

namespace Tests_BL_Backend.Repositories
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;
		public ProductRepositoryTests() 
        {
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			_context = new ApplicationDbContext(options);
			_repository = new ProductRepository(_context);

			SeedDataBase();
		}

        private void SeedDataBase()
        {
			var categories = new List<Category>
	        {
		        new Category { Id = 1, Name = "Computers" },
		        new Category { Id = 2, Name = "Mobile" },
		        new Category { Id = 3, Name = "Audio" }
	        };

            var variants = new List<ProductVariant>
            {
                new ProductVariant 
                { 
                    Id = 1, 
                    ProductId = 1, 
                    StockQuantity = 15,
                    Attributes = new List<ProductVariantAttribute>
                    {
                        new ProductVariantAttribute { AttributeName = "Condition", AttributeValue = "good" }
                    }
                },
                new ProductVariant 
                { 
                    Id = 2, 
                    ProductId = 2, 
                    StockQuantity = 30,
                    Attributes = new List<ProductVariantAttribute>
                    {
                        new ProductVariantAttribute { AttributeName = "Condition", AttributeValue = "very good" }
                    }
                }
            };

			_context.Categories.AddRange(categories);
            _context.ProductVariants.AddRange(variants);
			_context.SaveChanges();

			var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Description = "A powerful laptop", BasePrice = 999.99m, CategoryId = 1, Variants = variants },
                new Product { Id = 4, Name = "Tablet", Description = "The newest tablet", BasePrice = 899.99m, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Description = "A modern smartphone", BasePrice = 499.99m, CategoryId = 2, Variants = variants },
                new Product { Id = 3, Name = "Headphones", Description = "Noise-cancelling headphones", BasePrice = 199.99m, CategoryId = 3 }
            };

            _context.Products.AddRange(products);
            _context.SaveChanges();
		}
        [Fact]
        public async Task GetAllAsync_ReturnAllProducts()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(4, result.Count());
        }
        [Fact]
        public async Task GetByIdAsync_ReturnProductFound()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.Name);
		}
        [Fact]
        public async Task GetByIdAsync_ReturnProductIdIsNull()
        {
            // Act
            var product = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(product);
        }
        [Fact]
        public async Task GetByCategoryIdAsync()
        {
            // Act
            var result = await _repository.GetByCategoryIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async Task GetByCategoryIdAsync_ReturnEmpty()
        {
            // Act
            var result = await _repository.GetByCategoryIdAsync(999);

            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task SearchAsync_ReturnFound()
        {
            // Act
            var result = await _repository.SearchAsync("Laptop");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Laptop", result.First().Name);
        }
        [Fact]
        public async Task SearchAsync_ReturnAllIfNotFound()
        {
            // Act
            var result = await _repository.SearchAsync("");

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count());
        }
        [Fact]
        public async Task AddAsync_ReturnProduct()
        {
            // Arange
            Product product = new Product {
                Id = 5,
                Name = "Monitor",
                Description = "Gaming Monitor",
                BasePrice = 349.99m,
                CategoryId = 1
            };

            // Act
            var result = await _repository.AddAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
        }
        [Fact]
        public async Task AddAsync_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null));
        }
        [Fact]
        public async Task UpdateAsync_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null));
        }
        [Fact]
        public async Task UpdateAsync_ReturnUpdatedProduct()
        {
            // Arrange
            var productToUpdate = await _repository.GetByIdAsync(2);
            Assert.NotNull(productToUpdate);
            
            productToUpdate.Name = "Boxen";
            productToUpdate.Description = "Sehr laute Boxen";
            productToUpdate.BasePrice = 129.99m;

            // Act
            var result = await _repository.UpdateAsync(productToUpdate);

            // Assert - 1. Rückgabewert prüfen
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("Boxen", result.Name);
            Assert.Equal("Sehr laute Boxen", result.Description);
            Assert.Equal(129.99m, result.BasePrice);

            // Assert - 2. Persistenz prüfen: Update in DB bestätigen
            var updatedProduct = await _repository.GetByIdAsync(2);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Boxen", updatedProduct.Name);
            Assert.Equal("Sehr laute Boxen", updatedProduct.Description);
            Assert.Equal(129.99m, updatedProduct.BasePrice);
        }
        [Fact]
        public async Task DeleteAsync_DeleteProductSuccessfully()
        {
            // Arrange
            var productExists = await _repository.GetByIdAsync(1);
            Assert.NotNull(productExists);

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var deletedProduct = await _repository.GetByIdAsync(1);
            Assert.Null(deletedProduct);
        }
        [Fact]
        public async Task DeleteAsync_NonExistentId_NoException()
        {
            // Act & Assert
            await _repository.DeleteAsync(999);
            
            // Die Anzahl der Produkte sollte gleich bleiben
            var allProducts = await _repository.GetAllAsync();
            Assert.Equal(4, allProducts.Count());
        }
        [Fact]
        public async Task ExistsAsync_ReturnsTrue()
        {
            // Act and Assert
            Assert.True(await _repository.ExistsAsync(1));
        }
        [Fact]
        public async Task ExistsAsync_ReturnsFalse()
        {
            // Act and Assert
            Assert.False(await _repository.ExistsAsync(999));
        }
        [Fact]
        public async Task GetAvailableStockAsync_ReturnStock()
        {
            // Act
            var result = await _repository.GetAvailableStockAsync(1);

            // Assert
            Assert.Equal(45, result);
        }
        [Fact]
        public async Task GetAvailableStockAsync_ReturnZero()
        {
			// Act
			var result = await _repository.GetAvailableStockAsync(999);

			// Assert
			Assert.Equal(0, result);
		}
		public void Dispose()
        {
            _context.Database.EnsureDeleted();
			_context.Dispose();
		}
    }
}
