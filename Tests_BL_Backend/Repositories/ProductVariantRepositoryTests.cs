using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;

namespace Tests_BL_Backend.Repositories
{
    public class ProductVariantRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductVariantRepositry _repository;

        public ProductVariantRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new ProductVariantRepositry(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var variants = new List<ProductVariant>
            {
                new ProductVariant 
                { 
                    Id = 1, 
                    ProductId = 1, 
                    SKU = "TV-RED-42", 
                    StockQuantity = 150, 
                    PriceAdjustment = 50.00m,
                    Attributes = new List<ProductVariantAttribute>
                    {
                        new ProductVariantAttribute { Id = 1, AttributeName = "Color", AttributeValue = "Red" },
                        new ProductVariantAttribute { Id = 2, AttributeName = "Size", AttributeValue = "42 Zoll" }
                    }
                },
                new ProductVariant 
                { 
                    Id = 2, 
                    ProductId = 1, 
                    SKU = "TV-BLUE-55", 
                    StockQuantity = 5, 
                    PriceAdjustment = 100.00m,
                    Attributes = new List<ProductVariantAttribute>
                    {
                        new ProductVariantAttribute { Id = 3, AttributeName = "Color", AttributeValue = "Blue" },
                        new ProductVariantAttribute { Id = 4, AttributeName = "Size", AttributeValue = "55 Zoll" }
                    }
                },
                new ProductVariant 
                { 
                    Id = 3, 
                    ProductId = 2, 
                    SKU = "JACKET-RED-XL", 
                    StockQuantity = 0, 
                    PriceAdjustment = 0m,
                    Attributes = new List<ProductVariantAttribute>
                    {
                        new ProductVariantAttribute { Id = 5, AttributeName = "Color", AttributeValue = "Red" },
                        new ProductVariantAttribute { Id = 6, AttributeName = "Size", AttributeValue = "XL" },
                        new ProductVariantAttribute { Id = 7, AttributeName = "Material", AttributeValue = "Polyester" }
                    }
                },
                new ProductVariant 
                { 
                    Id = 4, 
                    ProductId = 2, 
                    SKU = "JACKET-GREEN-M", 
                    StockQuantity = 200, 
                    PriceAdjustment = -5.00m,
                    Attributes = new List<ProductVariantAttribute>
                    {
                        new ProductVariantAttribute { Id = 8, AttributeName = "Color", AttributeValue = "Green" },
                        new ProductVariantAttribute { Id = 9, AttributeName = "Size", AttributeValue = "M" }
                    }
                }
            };
            
            _context.ProductVariants.AddRange(variants);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllVariantsWithAttributes()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(4, result.Count());
            Assert.All(result, v => Assert.NotNull(v.Attributes));
            Assert.All(result, v => Assert.NotEmpty(v.Attributes));
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsVariantWithAttributes()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("TV-RED-42", result.SKU);
            Assert.Equal(2, result.Attributes.Count);
            Assert.Contains(result.Attributes, a => a.AttributeName == "Color" && a.AttributeValue == "Red");
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
        public async Task GetBySKUAsync_WithValidSKU_ReturnsVariant()
        {
            // Act
            var result = await _repository.GetBySKUAsync("TV-RED-42");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TV-RED-42", result.SKU);
            Assert.Equal(2, result.Attributes.Count);
        }

        [Fact]
        public async Task AddAsync_CreatesVariantWithAttributes()
        {
            // Arrange
            var newVariant = new ProductVariant
            {
                ProductId = 1,
                SKU = "NEW-SKU-001",
                StockQuantity = 50,
                PriceAdjustment = 25.00m,
                Attributes = new List<ProductVariantAttribute>
                {
                    new ProductVariantAttribute { AttributeName = "Color", AttributeValue = "Black" },
                    new ProductVariantAttribute { AttributeName = "Size", AttributeValue = "XXL" }
                }
            };

            // Act
            var result = await _repository.AddAsync(newVariant);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal("NEW-SKU-001", result.SKU);
            
            var allVariants = await _context.ProductVariants.Include(pv => pv.Attributes).ToListAsync();
            Assert.Equal(5, allVariants.Count);
            
            var created = allVariants.First(v => v.SKU == "NEW-SKU-001");
            Assert.Equal(2, created.Attributes.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesVariantAndAttributes()
        {
            // Arrange
            var variant = await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .FirstAsync(pv => pv.Id == 1);
            
            variant.StockQuantity = 999;
            variant.PriceAdjustment = 75.00m;
            variant.Attributes.First(a => a.AttributeName == "Color").AttributeValue = "Yellow";

            // Act
            var result = await _repository.UpdateAsync(variant);

            // Assert
            Assert.Equal(999, result.StockQuantity);
            Assert.Equal(75.00m, result.PriceAdjustment);
            
            var updated = await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .FirstAsync(pv => pv.Id == 1);
            Assert.Contains(updated.Attributes, a => a.AttributeName == "Color" && a.AttributeValue == "Yellow");
        }

        [Fact]
        public async Task DeleteAsync_RemovesVariantAndCascadesAttributes()
        {
            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            
            var deleted = await _context.ProductVariants.FindAsync(1);
            Assert.Null(deleted);
            
            var orphanedAttributes = await _context.ProductVariantAttributes
                .Where(a => a.ProductVariantId == 1)
                .ToListAsync();
            Assert.Empty(orphanedAttributes);
            
            var remaining = await _context.ProductVariants.ToListAsync();
            Assert.Equal(3, remaining.Count);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteAsync(999);

            // Assert
            Assert.False(result);
            
            var allVariants = await _context.ProductVariants.ToListAsync();
            Assert.Equal(4, allVariants.Count);
        }

        [Fact]
        public async Task GetHighQuantity_ReturnsVariantsAbove100()
        {
            // Act
            var result = await _repository.GetHighQuantity();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, v => Assert.True(v.StockQuantity > 100));
            Assert.Contains(result, v => v.SKU == "TV-RED-42");
            Assert.Contains(result, v => v.SKU == "JACKET-GREEN-M");
        }

        [Fact]
        public async Task GetLowQuantity_ReturnsVariantsBetween1And10()
        {
            // Act
            var result = await _repository.GetLowQuantity();

            // Assert
            Assert.Single(result);
            Assert.All(result, v => Assert.True(v.StockQuantity > 0 && v.StockQuantity <= 10));
            Assert.Equal("TV-BLUE-55", result.First().SKU);
        }

        [Fact]
        public async Task GetEmpty_ReturnsVariantsWithZeroStock()
        {
            // Act
            var result = await _repository.GetEmpty();

            // Assert
            Assert.Single(result);
            Assert.All(result, v => Assert.Equal(0, v.StockQuantity));
            Assert.Equal("JACKET-RED-XL", result.First().SKU);
        }

        [Fact]
        public async Task GetByAttributeAsync_WithExistingAttribute_ReturnsMatchingVariants()
        {
            // Act
            var result = await _repository.GetByAttributeAsync("Color", "Red");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, v => v.SKU == "TV-RED-42");
            Assert.Contains(result, v => v.SKU == "JACKET-RED-XL");
            Assert.All(result, v => 
                Assert.Contains(v.Attributes, a => a.AttributeName == "Color" && a.AttributeValue == "Red"));
        }

        [Fact]
        public async Task GetByAttributeAsync_WithNonExistentAttribute_ReturnsEmpty()
        {
            // Act
            var result = await _repository.GetByAttributeAsync("Color", "Purple");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByAttributeAsync_WithMaterial_ReturnsOnlyVariantsWithThatAttribute()
        {
            // Act
            var result = await _repository.GetByAttributeAsync("Material", "Polyester");

            // Assert
            Assert.Single(result);
            Assert.Equal("JACKET-RED-XL", result.First().SKU);
        }

        [Fact]
        public async Task AddAsync_WithoutAttributes_CreatesVariantSuccessfully()
        {
            // Arrange
            var variantWithoutAttributes = new ProductVariant
            {
                ProductId = 1,
                SKU = "SIMPLE-VARIANT",
                StockQuantity = 10,
                PriceAdjustment = 0m,
                Attributes = new List<ProductVariantAttribute>()
            };

            // Act
            var result = await _repository.AddAsync(variantWithoutAttributes);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Empty(result.Attributes);
        }

        [Fact]
        public async Task GetAllAsync_IncludesAllAttributesForEachVariant()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            var jacketRedXl = result.First(v => v.SKU == "JACKET-RED-XL");
            Assert.Equal(3, jacketRedXl.Attributes.Count);
            Assert.Contains(jacketRedXl.Attributes, a => a.AttributeName == "Material");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
