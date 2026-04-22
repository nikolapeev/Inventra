using Inventra.Core.Services;
using Inventra.Core.ViewModels.Couriers;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Inventra.Tests
{
    [TestFixture]
    public class CourierServiceTests
    {
        private InventraDbContext _context;
        private CourierService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new CourierService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldSuccessfullyAddCourier()
        {
            // Arrange
            var model = new CourierCreateViewModel
            {
                Name = "Econt",
                Phone = "0888123456"
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var courier = await _context.Couriers.FirstOrDefaultAsync();
            Assert.That(courier, Is.Not.Null);
            Assert.That(courier.Name, Is.EqualTo("Econt"));
            Assert.That(courier.Phone, Is.EqualTo("0888123456"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCouriersSortedByName()
        {
            // Arrange
            _context.Couriers.AddRange(new List<Courier>
            {
                new Courier { CourierId = Guid.NewGuid(), Name = "Speedy", Phone = "1" },
                new Courier { CourierId = Guid.NewGuid(), Name = "A1 Couriers", Phone = "2" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = (await _service.GetAllAsync()).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("A1 Couriers")); // Проверка на сортировката
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectCourier()
        {
            // Arrange
            var id = Guid.NewGuid();
            var courier = new Courier { CourierId = id, Name = "Leo Expres", Phone = "0999" };
            _context.Couriers.Add(courier);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CourierId, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo("Leo Expres"));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingCourierFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Couriers.Add(new Courier { CourierId = id, Name = "Old Name", Phone = "000" });
            await _context.SaveChangesAsync();

            var updateModel = new CourierIndexViewModel
            {
                CourierId = id,
                Name = "New Name",
                Phone = "111"
            };

            // Act
            await _service.UpdateAsync(updateModel);

            // Assert
            var updated = await _context.Couriers.FindAsync(id);
            Assert.That(updated.Name, Is.EqualTo("New Name"));
            Assert.That(updated.Phone, Is.EqualTo("111"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveCourierFromDb()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Couriers.Add(new Courier { CourierId = id, Name = "To Delete", Phone = "x" });
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            var exists = await _context.Couriers.AnyAsync(c => c.CourierId == id);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task UpdateAsync_WithNonExistentId_ShouldNotThrowException()
        {
            // Arrange
            var model = new CourierIndexViewModel { CourierId = Guid.NewGuid(), Name = "Ghost", Phone = "none" };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _service.UpdateAsync(model));
        }
    }
}