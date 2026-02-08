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
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Tech products" },
                new Category { Id = 2, Name = "Books", Description = "Reading materials" },
                new Category { Id = 3, Name = "Clothing", Description = "Fashion items" }
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
            await _repository.DeleteAsync(1);

            // Assert
            var deleted = await _context.Categories.FindAsync(1);
            Assert.Null(deleted);
            
            var remaining = await _context.Categories.ToListAsync();
            Assert.Equal(2, remaining.Count);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_DoesNotThrow()
        {
            // Act & Assert
            await _repository.DeleteAsync(999);
            
            var allCategories = await _context.Categories.ToListAsync();
            Assert.Equal(3, allCategories.Count);
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

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
