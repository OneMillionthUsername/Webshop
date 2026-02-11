using System.Collections;
using Webshop.Models;

namespace Webshop.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(bool includeProducts = false);
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<(int Id, string Name, int ProductCount)>> GetAllWithProductCountAsync();
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
	}
}
