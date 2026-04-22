using Inventra.Core.Services;
using Inventra.Core.ViewModels.Home;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Data.Enums;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Inventra.Tests
{
    [TestFixture]
    public class DashboardServiceTests
    {
        private InventraDbContext _context;
        private DashboardService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new DashboardService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetHomeStatsAsync_ShouldCalculateTotalRevenueCorrectly()
        {
            // Arrange
            var validCustomer = CreateValidCustomer();
            var validCourier = CreateValidCourier();

            _context.Orders.AddRange(new List<Order>
            {
                new Order { Id = Guid.NewGuid(), Customer = validCustomer, Courier = validCourier, TotalPrice = 100.50m, Status = Statuses.Processed, AdditionalInfo="Допълнително инфо 1", TrackingNumber="TRACK-123456", ETA = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = Guid.NewGuid(), Customer = validCustomer, Courier = validCourier, TotalPrice = 200.00m, Status = Statuses.Shipped, AdditionalInfo="Допълнително инфо 2", TrackingNumber="TRACK-987654", ETA = DateOnly.FromDateTime(DateTime.Now) }
            });
            await _context.SaveChangesAsync();

            // Act
            var stats = await _service.GetHomeStatsAsync();

            // Assert
            Assert.That(stats.TotalRevenue, Is.EqualTo(300.50m));
        }

        [Test]
        public async Task GetHomeStatsAsync_ShouldReturnTop5CustomersBySpending()
        {
            // Arrange
            var customer1 = CreateValidCustomer("Ivan-Ivanov", "Company-One");
            var customer2 = CreateValidCustomer("Maria-Petrova", "Company-Two");
            var courier = CreateValidCourier();

            _context.Orders.AddRange(new List<Order>
            {
                new Order { Id = Guid.NewGuid(), Customer = customer1, Courier = courier, TotalPrice = 500, Status = Statuses.Processed, AdditionalInfo="Информация за тест", TrackingNumber="TRK-1", ETA = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = Guid.NewGuid(), Customer = customer2, Courier = courier, TotalPrice = 1000, Status = Statuses.Processed, AdditionalInfo="Информация за тест", TrackingNumber="TRK-2", ETA = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = Guid.NewGuid(), Customer = customer1, Courier = courier, TotalPrice = 100, Status = Statuses.Processed, AdditionalInfo="Информация за тест", TrackingNumber="TRK-3", ETA = DateOnly.FromDateTime(DateTime.Now) }
            });
            await _context.SaveChangesAsync();

            // Act
            var stats = await _service.GetHomeStatsAsync();

            // Assert
            Assert.That(stats.TopCustomers.Count, Is.EqualTo(2));
            Assert.That(stats.TopCustomers[0].Name, Is.EqualTo("Maria-Petrova"));
            Assert.That(stats.TopCustomers[0].TotalSpent, Is.EqualTo(1000));
        }

        [Test]
        public async Task GetHomeStatsAsync_ShouldCorrectlyCountLowStockProducts()
        {
            // Arrange
            _context.Products.AddRange(new List<Product>
            {
                CreateValidProduct("P1", 2),  
                CreateValidProduct("P2", 4),  
                CreateValidProduct("P3", 10)  
            });
            await _context.SaveChangesAsync();

            // Act
            var stats = await _service.GetHomeStatsAsync();

            // Assert
            Assert.That(stats.LowStockProductsCount, Is.EqualTo(2));
        }

        [Test]
        public async Task GetHomeStatsAsync_ShouldFilterMessagesByTypeAndLimitCount()
        {
            // Arrange
            for (int i = 1; i <= 5; i++)
            {
                _context.Messages.Add(new Message
                {
                    Id = Guid.NewGuid(),
                    Type = MessageType.Crucial,
                    Content = $"Crucial message {i}",
                    CreatedAt = DateTime.Now.AddMinutes(i),
                    CreatedBy = "Admin"
                });
            }

            _context.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                Type = MessageType.Info,
                Content = "Info message",
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            });

            await _context.SaveChangesAsync();

            // Act
            var stats = await _service.GetHomeStatsAsync();

            // Assert
            Assert.That(stats.CrucialMessages.Count, Is.EqualTo(3));
            Assert.That(stats.CrucialMessages[0].CreatedAt, Is.GreaterThan(stats.CrucialMessages[1].CreatedAt));
        }

        [Test]
        public async Task GetHomeStatsAsync_ShouldCountOrdersByStatus()
        {
            // Arrange
            var customer = CreateValidCustomer();
            var courier = CreateValidCourier();

            _context.Orders.AddRange(new List<Order>
            {
                new Order { Id = Guid.NewGuid(), Customer = customer, Courier = courier, Status = Statuses.Processed, TotalPrice = 10, AdditionalInfo="Допълнително инфо", TrackingNumber="T-1", ETA = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = Guid.NewGuid(), Customer = customer, Courier = courier, Status = Statuses.Processed, TotalPrice = 10, AdditionalInfo="Допълнително инфо", TrackingNumber="T-2", ETA = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = Guid.NewGuid(), Customer = customer, Courier = courier, Status = Statuses.InProgress, TotalPrice = 10, AdditionalInfo="Допълнително инфо", TrackingNumber="T-3", ETA = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = Guid.NewGuid(), Customer = customer, Courier = courier, Status = Statuses.Shipped, TotalPrice = 10, AdditionalInfo="Допълнително инфо", TrackingNumber="T-4", ETA = DateOnly.FromDateTime(DateTime.Now) }
            });
            await _context.SaveChangesAsync();

            // Act
            var stats = await _service.GetHomeStatsAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(stats.TotalOrders, Is.EqualTo(4));
                Assert.That(stats.ProcessedOrdersCount, Is.EqualTo(2));
                Assert.That(stats.InProgressOrdersCount, Is.EqualTo(1));
                Assert.That(stats.ShippedOrdersCount, Is.EqualTo(1));
            });
        }


       

        private Customer CreateValidCustomer(string fullName = "Alexander-Test", string companyName = "Test-Co")
        {
            return new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = fullName,
                PhoneNumber = "0888123456",
                Email = "test@test.bg",
                Country = "Bulgaria",
                County = "Sofia-Region",
                City = "Sofia",
                Address = "Vitosha-100",
                PostalCode = "1000",
                EIK = "123456789",
                CompanyName = companyName,
                ZDDS = false
            };
        }

        private Courier CreateValidCourier()
        {
            return new Courier
            {
                CourierId = Guid.NewGuid(),
                Name = "Speedy",
                Phone = "0899123456" // Предполагам, че и тук може да има Required полета
            };
        }

        private Product CreateValidProduct(string name, int stockQuantity)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                StockQuantity = stockQuantity,
                Price = 10.0m,
                AddedBy = "System",
                BatchNumber = "B-12345",
                Description = "Много хубав продукт за тестване",
                ImageURL = "https://example.com/image.jpg",
                WarehouseLocationId = "A-12",
                
                Category = new Category { CategoryId = Guid.NewGuid(), Name = "Тест Категория" },
                Supplier = new Supplier { SupplierId = Guid.NewGuid(), Name = "Тест Доставчик",EIK="123456789" , Email="имейл@имейл.ком" , PhoneNumber="0879462525"}
            };
        }
    }
}