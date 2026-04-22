using Inventra.Core.Services;
using Inventra.Core.ViewModels.Suppliers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Tests
{
    [TestFixture]
    public class SupplierServiceTests
    {
        private InventraDbContext _context;
        private SupplierService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new SupplierService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldPersistSupplierData()
        {
            // Arrange
            var model = new SupplierCreateViewModel
            {
                Name = "Техно Лоджистик",
                EIK = "BG123456789",
                PhoneNumber = "0888111222",
                Email = "office@techno.bg"
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var supplier = await _context.Suppliers.FirstOrDefaultAsync();
            Assert.That(supplier, Is.Not.Null);
            Assert.That(supplier.Name, Is.EqualTo("Техно Лоджистик"));
            Assert.That(supplier.EIK, Is.EqualTo("BG123456789"));
        }

        [Test]
        public async Task GetAllSuppliers_ShouldReturnOrderedList()
        {
            // Arrange
            _context.Suppliers.AddRange(new List<Supplier>
            {
                new Supplier { SupplierId = Guid.NewGuid(), Name = "C" , EIK="123456789",Email="mail@mail.com",PhoneNumber="0879462525"},
                new Supplier { SupplierId = Guid.NewGuid(), Name = "A",EIK="123456789",Email="mail@mail.com",PhoneNumber="0879462525" },
                new Supplier {SupplierId = Guid.NewGuid(), Name = "B", EIK = "123456789", Email = "mail@mail.com", PhoneNumber = "0879462525"}
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllSuppliers();

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("A"));
            Assert.That(result[2].Name, Is.EqualTo("C"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectSupplierEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var supplier = new Supplier { SupplierId = id, Name = "Тест Доставчик", EIK = "123456789", Email = "mail@mail.com", PhoneNumber = "0879462525" };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SupplierId, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo("Тест Доставчик"));
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyExistingFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            var supplier = new Supplier { SupplierId = id, Name = "Old Name", Email = "old@test.bg" , EIK = "123456789", PhoneNumber = "0879462525" };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            var editModel = new SupplierEditViewModel
            {
                SupplierId = id,
                Name = "New Name",
                Email = "new@test.bg",
                EIK = "999999",
                PhoneNumber = "123"
            };

            // Act
            await _service.UpdateAsync(editModel);

            // Assert
            var updated = await _context.Suppliers.FindAsync(id);
            Assert.That(updated.Name, Is.EqualTo("New Name"));
            Assert.That(updated.Email, Is.EqualTo("new@test.bg"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveSupplierSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Suppliers.Add(new Supplier { SupplierId = id, Name = "За триене",EIK="123456789",Email="mail@mail.com",PhoneNumber="0879462525" });
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            var exists = await _context.Suppliers.AnyAsync(s => s.SupplierId == id);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task UpdateAsync_WithNonExistentId_ShouldNotThrow()
        {
            // Arrange
            var model = new SupplierEditViewModel { SupplierId = Guid.NewGuid() };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _service.UpdateAsync(model));
        }
    }
}