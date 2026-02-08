using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Models;

namespace Webshop.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            
            if (customer != null && customer.IsAnonymized != true)
            {
                customer.FirstName = "deleted";
                customer.LastName = "deleted";
                customer.Email = "deleted";
                customer.Address = "deleted";
                customer.City = "deleted";
                customer.PhoneNumber = "deleted";
                customer.PostalCode = "deleted";
                customer.RegistrationDate = DateTime.Now;
                customer.IsActive = false;
                customer.IsAnonymized = true;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(customer => customer.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
