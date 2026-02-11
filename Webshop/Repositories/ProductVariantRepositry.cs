using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Repositories
{
    public class ProductVariantRepositry : IProductVariantRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantRepositry(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariant?> GetByIdAsync(int id)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .FirstOrDefaultAsync(pv => pv.Id == id);
        }

        public async Task<ProductVariant?> GetBySKUAsync(int sku)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .FirstOrDefaultAsync(pv => pv.SKU == sku.ToString());
        }

        public async Task<ProductVariant> AddAsync(ProductVariant variant)
        {
            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();
            return variant;
        }

        public async Task<ProductVariant> UpdateAsync(ProductVariant variant)
        {
            _context.ProductVariants.Update(variant);
            await _context.SaveChangesAsync();
            return variant;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var variant = await _context.ProductVariants.FindAsync(id);
            if (variant == null)
                return false;

            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductVariant>> GetHighQuantity()
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .Where(pv => pv.StockQuantity > 100)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductVariant>> GetLowQuantity()
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .Where(pv => pv.StockQuantity > 0 && pv.StockQuantity <= 10)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductVariant>> GetEmpty()
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .Where(pv => pv.StockQuantity == 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductVariant>> GetByAttributeAsync(string attributeName, string attributeValue)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Attributes)
                .Where(pv => pv.Attributes.Any(a => 
                    a.AttributeName == attributeName && 
                    a.AttributeValue == attributeValue))
                .ToListAsync();
        }
    }
}
