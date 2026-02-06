using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Dtos.Categories;
using Webshop.Dtos.Products;
using Webshop.Models;

namespace Webshop.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    CategoryId = p.CategoryId
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                BasePrice = createDto.BasePrice,
                CategoryId = createDto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId
            };
        }

        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto)
        {
            var product = await _context.Products.FindAsync(updateDto.Id);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {updateDto.Id} not found.");

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.BasePrice = updateDto.BasePrice;
            product.CategoryId = updateDto.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                CategoryId = product.CategoryId
            };
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(e => e.Id == productId);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    CategoryId = p.CategoryId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            return await _context.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    CategoryId = p.CategoryId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();
        }

        public async Task<bool> CheckInventoryAsync(int productId, int quantity)
        {
            var availableStock = await GetAvailableStockAsync(productId);
            return availableStock >= quantity;
        }

        public async Task<int> GetAvailableStockAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return 0;

            return product.Variants?.Sum(v => v.StockQuantity) ?? 0;
        }

        public async Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync()
        {
            return await _context.Products
                .Take(10)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    CategoryId = p.CategoryId
                })
                .ToListAsync();
        }
    }
}
