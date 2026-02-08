using Webshop.Models;

namespace Webshop.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByOrderIdAsync(int orderId);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
