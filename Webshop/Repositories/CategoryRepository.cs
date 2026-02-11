using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool includeProducts = false)
        {
            var query = _context.Categories.AsQueryable();

            if(includeProducts)
                query = query.Include(c => c.Products);

            return await query.ToListAsync();
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(e => e.Id == id);
        }
        public async Task<IEnumerable<(int Id, string Name, int ProductCount)>> GetAllWithProductCountAsync()
        {
            return (await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Products)
                .AsNoTracking()
                .ToListAsync())
                .Select(c => (c.Id, c.Name, c.Products.Count));
        }
        public Task<Category?> GetByNameAsync(string name) 
        {  
            throw new NotImplementedException(); 
        }
        public Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            throw new NotImplementedException(); 
        }
    }
}
