using Webshop.Models;

namespace Webshop.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<Customer> AddAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustomerByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Customer>? GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer>? GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Customer>? GetByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
