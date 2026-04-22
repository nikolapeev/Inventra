using Inventra.Core.Services;
using Inventra.Core.ViewModels.OrderDetails;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Tests
{
    [TestFixture]
    public class OrderDetailsServiceTests
    {
        private InventraDbContext _context;
        private OrderDetailsService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new OrderDetailsService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_NewItem_ShouldReduceStockAndIncreaseOrderTotal()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            _context.Orders.Add(new Order { Id = orderId, TotalPrice = 0 , AdditionalInfo = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", TrackingNumber = "123wert5" });
            _context.Products.Add(new Product { Id = productId, Name = "Laptop", Price = 1000, StockQuantity = 10 , AddedBy = "System", BatchNumber = "20-2026B", Description = "qwertyuiopasdfghjklz", ImageURL = "Folder/Folder1/Image.jpg", WarehouseLocationId = "A1-23" });
            await _context.SaveChangesAsync();

            var model = new OrderDetailsCreateViewModel { OrderId = orderId, ProductId = productId, QTY = 2 };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var product = await _context.Products.FindAsync(productId);
            var order = await _context.Orders.FindAsync(orderId);
            var detail = await _context.OrderDetails.FirstAsync();

            Assert.That(product.StockQuantity, Is.EqualTo(8)); // 10 - 2
            Assert.That(order.TotalPrice, Is.EqualTo(2000));   // 2 * 1000
            Assert.That(detail.Subtotal, Is.EqualTo(2000));
        }

        [Test]
        public async Task CreateAsync_ExistingItem_ShouldUpdateQuantityAndTotal()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var product = new Product { Id = productId, Name = "Mouse", Price = 50, StockQuantity = 100, AddedBy="System" , BatchNumber="20-2026B", Description="qwertyuiopasdfghjklz", ImageURL="Folder/Folder1/Image.jpg", WarehouseLocationId="A1-23" };
            var order = new Order { Id = orderId, TotalPrice = 50 , AdditionalInfo="123456789qwerty" , TrackingNumber="qw1234qw23"};
            var existingDetail = new OrderDetails { OrderId = orderId, ProductId = productId, QTY = 1, Subtotal = 50 };

            _context.Products.Add(product);
            _context.Orders.Add(order);
            _context.OrderDetails.Add(existingDetail);
            await _context.SaveChangesAsync();

            var model = new OrderDetailsCreateViewModel { OrderId = orderId, ProductId = productId, QTY = 2 };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var updatedDetail = await _context.OrderDetails.FirstAsync();
            Assert.That(updatedDetail.QTY, Is.EqualTo(3)); // 1 + 2
            Assert.That(product.StockQuantity, Is.EqualTo(98)); // 100 - 2
        }

        [Test]
        public async Task CreateAsync_InsufficientStock_ShouldReturnWithoutChanges()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            _context.Orders.Add(new Order { Id = orderId, TotalPrice = 0 , AdditionalInfo="aaaaaaaaaaaaaaaaaaaaaaaaaaaaa" , TrackingNumber="123wert5"});
            _context.Products.Add(new Product { Id = productId, Price = 10, Name="dealdough", StockQuantity = 1, AddedBy = "System", BatchNumber = "20-2026B", Description = "qwertyuiopasdfghjklz", ImageURL = "Folder/Folder1/Image.jpg", WarehouseLocationId = "A1-23" }); 
            await _context.SaveChangesAsync();

            var model = new OrderDetailsCreateViewModel { OrderId = orderId, ProductId = productId, QTY = 5 };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var product = await _context.Products.FindAsync(productId);
            Assert.That(product.StockQuantity, Is.EqualTo(1)); // Не се е променило
            Assert.That(await _context.OrderDetails.AnyAsync(), Is.False);
        }

        [Test]
        public async Task DeleteAsync_ShouldRestoreStockAndDecreaseOrderTotal()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var product = new Product { Id = productId, Price = 100, StockQuantity = 5, Name="sinep" ,AddedBy = "System", BatchNumber = "20-2026B", Description = "qwertyuiopasdfghjklz", ImageURL = "Folder/Folder1/Image.jpg", WarehouseLocationId = "A1-23" };
            var order = new Order { Id = orderId, TotalPrice = 200, AdditionalInfo = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", TrackingNumber = "123wert5" };
            var detail = new OrderDetails { OrderId = orderId, ProductId = productId, QTY = 2, Subtotal = 200 };

            _context.Products.Add(product);
            _context.Orders.Add(order);
            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(orderId, productId);

            // Assert
            Assert.That(product.StockQuantity, Is.EqualTo(7)); // 5 + 2
            Assert.That(order.TotalPrice, Is.EqualTo(0));     // 200 - 200
            Assert.That(await _context.OrderDetails.AnyAsync(), Is.False);
        }

        [Test]
        public async Task UpdateAsync_IncreasingQuantity_ShouldAdjustStockAndTotal()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var product = new Product { Id = productId, Price = 10, StockQuantity = 10, Name = "sinep", AddedBy = "System", BatchNumber = "20-2026B", Description = "qwertyuiopasdfghjklz", ImageURL = "Folder/Folder1/Image.jpg", WarehouseLocationId = "A1-23" };
            var order = new Order { Id = orderId, TotalPrice = 20, AdditionalInfo = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", TrackingNumber = "123wert5" };
            var detail = new OrderDetails { OrderId = orderId, ProductId = productId, QTY = 2, Subtotal = 20 };

            _context.Products.Add(product);
            _context.Orders.Add(order);
            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();

            var editModel = new OrderDetailsEditViewModel { OrderId = orderId, ProductId = productId, QTY = 5 };

            // Act
            await _service.UpdateAsync(editModel);

            // Assert
            Assert.That(detail.QTY, Is.EqualTo(5));
            Assert.That(product.StockQuantity, Is.EqualTo(7)); // 10 - (5-2)
            Assert.That(order.TotalPrice, Is.EqualTo(50));     // 20 + (3 * 10)
        }
    }
}