using Inventra.Core.Services;
using Inventra.Core.ViewModels.Customers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Inventra.Tests
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private InventraDbContext _context;
        private CustomerService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new CustomerService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldMapAllFieldsCorrectly()
        {
            // Arrange
            var model = new CustomerCreateViewModel
            {
                FullName = "Ivan-Ivanov",
                PhoneNumber = "0888123456",
                Email = "ivan@test.com",
                Country = "Bulgaria",
                County = "Sofia-Region",
                City = "Sofia",
                Address = "Vitosha-100",
                PostalCode = "1000",
                EIK = "123456789",
                CompanyName = "Progres-OOD",
                ZDDS = true
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var customer = await _context.Customers.FirstOrDefaultAsync();
            Assert.That(customer, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(customer.FullName, Is.EqualTo(model.FullName));
                Assert.That(customer.PhoneNumber, Is.EqualTo(model.PhoneNumber));
                Assert.That(customer.Email, Is.EqualTo(model.Email));
                Assert.That(customer.Country, Is.EqualTo(model.Country));
                Assert.That(customer.County, Is.EqualTo(model.County));
                Assert.That(customer.City, Is.EqualTo(model.City));
                Assert.That(customer.Address, Is.EqualTo(model.Address));
                Assert.That(customer.PostalCode, Is.EqualTo(model.PostalCode));
                Assert.That(customer.EIK, Is.EqualTo(model.EIK));
                Assert.That(customer.CompanyName, Is.EqualTo(model.CompanyName));
                Assert.That(customer.ZDDS, Is.EqualTo(model.ZDDS));
            });
        }

        [Test]
        public async Task GetAllAsync_WithSearchTerm_ShouldFilterByFullName()
        {
            // Arrange
            _context.Customers.AddRange(new List<Customer>
            {
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Alexander-Test", CompanyName = "Comp-A", PhoneNumber = "1", Email = "a@a.bg", Country = "BG", County = "C", City = "C", Address = "Add-1", PostalCode = "111", EIK = "111111111", ZDDS = false },
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Georgi-Test", CompanyName = "Comp-B", PhoneNumber = "2", Email = "b@b.bg", Country = "BG", County = "C", City = "C", Address = "Add-2", PostalCode = "222", EIK = "222222222", ZDDS = false }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync("Alexander");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FullName, Is.EqualTo("Alexander-Test"));
        }

        [Test]
        public async Task GetAllAsync_WithSearchTerm_ShouldFilterByCompanyName()
        {
            // Arrange
            _context.Customers.Add(new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = "Human-Test",
                CompanyName = "SoftUni",
                PhoneNumber = "123",
                Email = "test@softuni.bg",
                Country = "Bulgaria",
                County = "Sofia",
                City = "Sofia",
                Address = "Tinterova-15",
                PostalCode = "1000",
                EIK = "123456789",
                ZDDS = true
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync("Soft");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CompanyName, Is.EqualTo("SoftUni"));
        }

        [Test]
        public async Task GetAllAsync_WithoutSearchTerm_ShouldReturnEverything()
        {
            // Arrange
            _context.Customers.AddRange(new List<Customer>
            {
                new Customer { CustomerId = Guid.NewGuid(), FullName = "A-Name", CompanyName = "A-Comp", PhoneNumber = "1", Email = "a@a.bg", Country = "BG", County = "C", City = "C", Address = "Add-1", PostalCode = "111", EIK = "111111111", ZDDS = false },
                new Customer { CustomerId = Guid.NewGuid(), FullName = "B-Name", CompanyName = "B-Comp", PhoneNumber = "2", Email = "b@b.bg", Country = "BG", County = "C", City = "C", Address = "Add-2", PostalCode = "222", EIK = "222222222", ZDDS = false }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync(null);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnDetailsCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Customers.Add(new Customer
            {
                CustomerId = id,
                FullName = "Maria-Test",
                CompanyName = "Maria-Trade",
                PhoneNumber = "0888999999",
                Email = "maria@test.bg",
                Country = "Bulgaria",
                County = "Sofia",
                City = "Sofia",
                Address = "Center-1",
                PostalCode = "1000",
                EIK = "987654321",
                ZDDS = true
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.FullName, Is.EqualTo("Maria-Test"));
                Assert.That(result.City, Is.EqualTo("Sofia"));
                Assert.That(result.EIK, Is.EqualTo("987654321"));
            });
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyExistingCustomer()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Customers.Add(new Customer
            {
                CustomerId = id,
                FullName = "Old-Name",
                CompanyName = "Old-Comp",
                PhoneNumber = "0000",
                Email = "old@bg",
                Country = "BG",
                County = "X",
                City = "Plovdiv",
                Address = "X-1",
                PostalCode = "4000",
                EIK = "111111111",
                ZDDS = true
            });
            await _context.SaveChangesAsync();

            var updateModel = new CustomerIndexViewModel
            {
                CustomerId = id,
                FullName = "New-Name",
                PhoneNumber = "0888222333",
                Email = "new@test.com",
                Country = "Bulgaria",
                County = "Varna-Region",
                City = "Varna",
                Address = "Sea-Garden-5",
                PostalCode = "9000",
                EIK = "222222222",
                CompanyName = "New-Comp",
                ZDDS = false
            };

            // Act
            await _service.UpdateAsync(updateModel);

            // Assert
            var updated = await _context.Customers.FindAsync(id);
            Assert.Multiple(() =>
            {
                Assert.That(updated.FullName, Is.EqualTo("New-Name"));
                Assert.That(updated.City, Is.EqualTo("Varna"));
                Assert.That(updated.EIK, Is.EqualTo("222222222"));
                Assert.That(updated.ZDDS, Is.EqualTo(false));
            });
        }

        [Test]
        public async Task DeleteAsync_ShouldActuallyRemoveRecord()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Customers.Add(new Customer
            {
                CustomerId = id,
                FullName = "Delete-Me",
                CompanyName = "X-Comp",
                PhoneNumber = "0",
                Email = "x@x",
                Country = "X",
                County = "X",
                City = "X",
                Address = "X-1",
                PostalCode = "1",
                EIK = "123456789",
                ZDDS = false
            });
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            var exists = await _context.Customers.AnyAsync(c => c.CustomerId == id);
            Assert.That(exists, Is.False);
        }
    }
}