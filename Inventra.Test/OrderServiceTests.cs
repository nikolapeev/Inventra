using Inventra.Core.Services;
using Inventra.Core.ViewModels.Orders;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private InventraDbContext _context;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new OrderService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        [Test]
        public async Task GetAllOrders_ShouldFilterByTrackingNumber()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = "Иван",
                CompanyName = "Епъл",
                Address = "Qnkova 8",
                City = "Kazanlak",
                County = "Zara Stagora",
                Country = "Sheinovo",
                EIK = "123456789",
                Email = "email@email.com",
                PhoneNumber = "0879462525",
                PostalCode = "6100"
            };

            var courier = new Courier { CourierId = Guid.NewGuid(), Name = "Спиди", Phone="0879462525" }; // Създаваме тестов куриер

            _context.Orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                Courier = courier, // Закачаме куриера към поръчката
                TrackingNumber = "TRACK123",
                Status = Statuses.InProgress,
                AdditionalInfo = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            });
            await _context.SaveChangesAsync();

            // Act
            var searchByTrack = await _service.GetAllOrders("TRACK123");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(searchByTrack.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetDetailsById_ShouldReturnCompleteViewModelWithProducts()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customer = new Customer {
                CustomerId = Guid.NewGuid(),
                FullName = "Иван",
                CompanyName = "Епъл",
                Address = "Qnkova 8",
                City = "Kazanlak",
                County = "Zara Stagora",
                Country = "Sheinovo",
                EIK = "123456789",
                Email = "email@email.com",
                PhoneNumber = "0879462525",
                PostalCode = "6100"
            };
            var courier = new Courier { CourierId = Guid.NewGuid(), Name = "Еконт", Phone="0879462525" };
            var product = new Product { Id = Guid.NewGuid(), Name = "Мишка", Price = 10 , AddedBy="System", BatchNumber="2-2026", 
                Description="aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ImageURL="Folder/Folder1/image.jpeg" , StockQuantity=10, WarehouseLocationId="A1-2"};

            _context.Orders.Add(new Order { Id = orderId, Customer = customer, Courier = courier , AdditionalInfo="aaaaaaaaaaaaaa", Status=Statuses.InProgress, TotalPrice=0, TrackingNumber="aaaaawdeasdaw"  });
            _context.OrderDetails.Add(new OrderDetails { OrderId = orderId, Product = product, QTY = 2, Subtotal = 20 });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetDetailsById(orderId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CustomerName, Is.EqualTo("Иван"));
            Assert.That(result.Products.Count, Is.EqualTo(1));
            Assert.That(result.Products[0].Product.Name, Is.EqualTo("Мишка"));
        }

        [Test]
        public async Task SumOrderTotal_ShouldCalculateCorrectSumFromDetails()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _context.OrderDetails.AddRange(new List<OrderDetails>
            {
                new OrderDetails { OrderId = orderId, ProductId = Guid.NewGuid(), Subtotal = 50.50m, QTY = 1 },
                new OrderDetails { OrderId = orderId, ProductId = Guid.NewGuid(), Subtotal = 49.50m, QTY = 1 }
            });
            await _context.SaveChangesAsync();

            // Act
            var total = await _service.SumOrderTotal(orderId);

            // Assert
            Assert.That(total, Is.EqualTo(100.00m));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateAllFieldsCorrectly()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, TrackingNumber = "OLD", Status = Statuses.InProgress, AdditionalInfo="aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var model = new OrderIndexViewModel
            {
                Id = orderId,
                TrackingNumber = "NEW",
                Status = Statuses.Shipped
            };

            // Act
            await _service.UpdateAsync(model);

            // Assert
            var updated = await _context.Orders.FindAsync(orderId);
            Assert.That(updated.TrackingNumber, Is.EqualTo("NEW"));
            Assert.That(updated.Status, Is.EqualTo(Statuses.Shipped));
        }

        [Test]
        public async Task OrderExists_ShouldReturnCorrectBoolean()
        {
            // Arrange
            var id = Guid.NewGuid();
            Guid courierId= Guid.NewGuid();
            Courier courier = new Courier { CourierId= courierId, Name="sinep" , Phone="088888888888888"};
            _context.Orders.Add(new Order { Id = id , AdditionalInfo="ааааааааааааааааааавааааааааа" , TrackingNumber="а2313ва4я2", CourierId=courierId});
            await _context.SaveChangesAsync();

            // Act & Assert
            Assert.That(_service.OrderExists(id), Is.True);
            Assert.That(_service.OrderExists(Guid.NewGuid()), Is.False);
        }
    }
}