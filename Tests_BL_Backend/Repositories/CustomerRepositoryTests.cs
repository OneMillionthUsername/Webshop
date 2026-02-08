using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Webshop.Data;
using Webshop.Models;
using Webshop.Repositories;
using static NuGet.Packaging.PackagingConstants;

namespace Tests_BL_Backend.Repositories
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new CustomerRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
			var orders = new List<Order>
			{
				new Order { Id = 1, CustomerId = 1, Items = {}, OrderDate=DateTime.Parse("29.11.2025"), TotalAmount = 1149.84m},
				new Order { Id = 2, CustomerId = 1, Items = {}, OrderDate=DateTime.Parse("15.06.2023"), TotalAmount = 110m}
			};

            _context.AddRange(orders);
            _context.SaveChanges();

			var customers = new List<Customer>
			{
				new Customer { Id = 1, FirstName = "Johnny", LastName = "Bravo", Address = "Beautystreet 29/5", City="Vienna", Email="johnny.bravo@gmail.com", PostalCode="1050", PhoneNumber="+436981654875", RegistrationDate=DateTime.Parse("12.05.2020"), Orders = orders},
				new Customer { Id = 2, FirstName = "Bonda", LastName = "Sukumba", Address = "Pupuleto 3", City="Imbudu", Email="paransa@kongo.kg", PostalCode="8745", PhoneNumber="+64223461262", RegistrationDate=DateTime.Parse("19.03.2017"), Orders = orders}
            };

            _context.AddRange(customers);
            _context.SaveChanges();
		}

        [Fact]
        public async Task GetAllAsync_ReturnAll()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async Task ExistsAsync_ReturnsTrue()
        {
            // Act and Assert
            Assert.True(await _repository.ExistsAsync(1));
        }
        [Fact]
		public async Task ExistsAsync_ReturnsFalse()
		{
			// Act and Assert
			Assert.False(await _repository.ExistsAsync(999));
		}
        [Fact]
        public async Task AddAsync_ReturnsCustomer()
        {
            // Arrange
            Customer customer = new Customer { 
                Id = 3,
                FirstName = "Some",
                LastName = "Guy",
                Address = "Bullstreet 12-24",
                City = "New York",
                PostalCode = "15235",
                Email = "some.guy@gmail.com",
                PhoneNumber = "+16597145665",
                RegistrationDate = DateTime.Now

            };
            // Act
            var result = await _repository.AddAsync(customer); 

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.FirstName, result.FirstName);
            Assert.Equal(customer.Id, result.Id);
        }
        [Fact]
        public async Task AddAsync_ThrowsException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null));
        }
        [Fact]
        public async Task DeleteByIdAsync_Successfully()
        {
            // Arrange 
            var customer = await _repository.GetByIdAsync(1);
            Assert.NotNull(customer);
            Assert.False(customer.IsAnonymized);
            Assert.True(customer.IsActive);
            Assert.Equal("Johnny", customer.FirstName);

            // Act
            await _repository.DeleteByIdAsync(1);

            // Assert
            var deletedCustomer = await _repository.GetByIdAsync(1);
            Assert.NotNull(deletedCustomer);
            Assert.True(deletedCustomer.IsAnonymized);
            Assert.False(deletedCustomer.IsActive);
            Assert.Equal("deleted", deletedCustomer.FirstName);
        }
        [Fact]
        public async Task DeleteByIdAsync_IsAnonymized()
        {
            // Arragne
            var customer = new Customer
            {
                Id = 3,
                FirstName = "deleted",
                LastName = "deleted",
                Email = "deleted",
                Address = "deleted",
                City = "deleted",
                PhoneNumber = "deleted",
                PostalCode = "deleted",
                RegistrationDate = DateTime.Parse("12.12.2022"),
                Orders = { },
                IsActive = false,
                IsAnonymized = true
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Act
            await _repository.DeleteByIdAsync(3);
            var deletedCustomer = await _repository.GetByIdAsync(3);

            //Assert
            Assert.NotNull(deletedCustomer);
            Assert.True(deletedCustomer.IsAnonymized);
            Assert.False(deletedCustomer.IsActive);
            Assert.Equal(customer.RegistrationDate, deletedCustomer.RegistrationDate);
            Assert.Equal(customer.FirstName, deletedCustomer.FirstName);
		}
        [Fact]
        public async Task DeleteByIdAsync_WrongId()
        {
            // Act
            await _repository.DeleteByIdAsync(999);
            var deletedCustomer = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(deletedCustomer);
        }
        [Fact]
        public async Task GetByIdAsync_ReturnsCustomer()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsActive);
            Assert.Equal(1, result.Id);
            Assert.False(result.IsAnonymized);
            Assert.Equal("Johnny", result.FirstName);
        }
		[Fact]
		public async Task GetByIdAsync_ReturnsNull()
		{
			// Act
			var result = await _repository.GetByIdAsync(999);

			// Assert
			Assert.Null(result);
		}
        [Fact]
        public async Task GetByOrderIdAsync_ReturnsCustomer()
        {
            // Act
            var result = await _repository.GetByOrderIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Bravo", result.LastName);
        }
		[Fact]
		public async Task GetByOrderIdAsync_ReturnsNull()
		{
			// Act
			var result = await _repository.GetByOrderIdAsync(999);

			// Assert
			Assert.Null(result);
		}
        [Fact]
        public async Task UpdateAsync_ReturnsUpdated()
        {
            // Arrange
            var customer = await _repository.GetByIdAsync(1);
            Assert.NotNull(customer);
            Assert.Equal("Johnny", customer.FirstName);

            // Act
            customer.FirstName = "Test";
            await _repository.UpdateAsync(customer);

            var updatedCustomer = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal(customer.FirstName, updatedCustomer.FirstName);
        }
		[Fact]
		public async Task UpdateAsync_ThrowsException()
		{
			// Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await  _repository.UpdateAsync(null));
		}
		public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
