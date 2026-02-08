using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;

namespace Tests_BL_Backend.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new CategoryRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var variants = new List<ProductVariant>
            {
                new ProductVariant { Id = 1, ProductId = 1, StockQuantity = 10, SKU="A1", Attributes="21 Zoll" },
                new ProductVariant { Id = 2, ProductId = 1, StockQuantity = 5, SKU="B", Attributes="27 Zoll" },
                new ProductVariant { Id = 3, ProductId = 1, StockQuantity = 20, SKU="C", Attributes="42 Zoll" },
				new ProductVariant { Id = 4, ProductId = 2, StockQuantity = 100, SKU="D12", Attributes="Blau" }
			};
            _context.ProductVariants.AddRange(variants);
            _context.SaveChanges();
            var products = new List<Product>
            {
                new Product { Id = 1, CategoryId = 1, Name="Samsung Smart TV", BasePrice=499.99m, Description="48 Zoll", Variants=variants},
                new Product { Id = 2, CategoryId = 3, Name="Regenjacke", BasePrice=99.99m, Description="Wasserdicht", Variants=variants}
            };
            _context.Products.AddRange(products);
            _context.SaveChanges();
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Tech products", Products = products, IsActive = true },
                new Category { Id = 2, Name = "Books", Description = "Reading materials", Products = { }, IsActive = false },
                new Category { Id = 3, Name = "Clothing", Description = "Fashion items", Products = products, IsActive = true }
            };
            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsCategory()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Electronics", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_CreatesCategory()
        {
            // Arrange
            var newCategory = new Category { Name = "New Category", Description = "New Desc" };

            // Act
            var result = await _repository.AddAsync(newCategory);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal("New Category", result.Name);
            
            var allCategories = await _context.Categories.ToListAsync();
            Assert.Equal(4, allCategories.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingCategory()
        {
            // Arrange
            var category = await _context.Categories.FindAsync(1);
            category!.Name = "Updated Electronics";
            category.Description = "Updated Description";

            // Act
            var result = await _repository.UpdateAsync(category);

            // Assert
            Assert.Equal("Updated Electronics", result.Name);
            Assert.Equal("Updated Description", result.Description);
            
            var updated = await _context.Categories.FindAsync(1);
            Assert.Equal("Updated Electronics", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesCategory()
        {
            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            var deleted = await _context.Categories.FindAsync(1);
            Assert.Null(deleted);
            
            var remaining = await _context.Categories.ToListAsync();
            Assert.Equal(2, remaining.Count);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_DoesNotThrow()
        {
            // Act & Assert
            var result = await _repository.DeleteAsync(999);
            
            var allCategories = await _context.Categories.ToListAsync();
            Assert.Equal(3, allCategories.Count);
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistentId_ReturnsFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetWithProductCountAsync_ReturnsCount()
        {
            // Act
            var result = await _repository.GetAllWithProductCountAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

		public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
