using Webshop.Models;

namespace Webshop.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer>? GetByIdAsync(int id);
        Task<Customer>? GetByNameAsync(string name);
        Task<Customer>? GetByOrderIdAsync(int orderId);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteCustomerByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
