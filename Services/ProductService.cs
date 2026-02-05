using Microsoft.EntityFrameworkCore;
using Webshop.Data;
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

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == productId)
                ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Variants)
                .Where(p => EF.Property<int>(p, "CategoryId") == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _context.Products.Include(p => p.Variants).ToListAsync();

            return await _context.Products
                .Include(p => p.Variants)
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
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

            return product.Variants.Sum(v => v.StockQuantity);
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            // Beispiel: Top 10 Produkte (kann später durch Featured-Flag ersetzt werden)
            return await _context.Products
                .Include(p => p.Variants)
                .Take(10)
                .ToListAsync();
        }
    }
}
