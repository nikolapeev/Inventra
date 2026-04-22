using Inventra.Core.Services;
using Inventra.Core.ViewModels.Products;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private InventraDbContext _context;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new ProductService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldSetAddedByToSystem_WhenUserIsNull()
        {
            // Arrange
            var model = new ProductCreateViewModel
            {
                Name = "Тестов продукт",
                Price = 10.50m,
                StockQuantity = 5,
                BatchNumber = "2-2026",
                Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2"
            };

            // Act
            await _service.CreateAsync(model, null);

            // Assert
            var product = await _context.Products.FirstOrDefaultAsync();
            Assert.That(product.AddedBy, Is.EqualTo("System"));
        }

        [Test]
        public async Task CreateAsync_ShouldSetSpecificUser_WhenProvided()
        {
            // Arrange
            var model = new ProductCreateViewModel
            {
                Name = "Продукт X",
                BatchNumber = "2-2026",
                Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2"
            };
            string userName = "AdminUser";

            // Act
            await _service.CreateAsync(model, userName);

            // Assert
            var product = await _context.Products.FirstOrDefaultAsync();
            Assert.That(product.AddedBy, Is.EqualTo("AdminUser"));
        }

        [Test]
        public async Task GetAllAsync_ShouldFilterByName()
        {
            // Arrange
            var cat = new Category { CategoryId = Guid.NewGuid(), Name = "IT" };
            var sup = new Supplier { SupplierId = Guid.NewGuid(), Name = "Dell", EIK = "123456788", Email = "email@email.com", PhoneNumber = "0879462525" };
            _context.Categories.Add(cat);
            _context.Suppliers.Add(sup);

            _context.Products.AddRange(new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Monitor", Category = cat, Supplier = sup,
                BatchNumber = "2-2026",
                Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2",
                AddedBy="System"},
                new Product { Id = Guid.NewGuid(), Name = "Keyboard", Category = cat, Supplier = sup ,
                    BatchNumber = "2-2026",
                Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2",
                AddedBy="System"}
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync("Mon");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Monitor"));
        }

        [Test]
        public async Task GetDetailsByIdAsync_ShouldReturnCorrectDataWithRelations()
        {
            // Arrange
            var catId = Guid.NewGuid();
            var supId = Guid.NewGuid();
            var prodId = Guid.NewGuid();

            _context.Categories.Add(new Category { CategoryId = catId, Name = "Hardware" });
            _context.Suppliers.Add(new Supplier { SupplierId = supId, Name = "HP", EIK = "123456788", Email = "email@email.com", PhoneNumber = "0879462525" });
            _context.Products.Add(new Product
            {
                Id = prodId,
                Name = "Laptop",
                CategoryId = catId,
                SupplierId = supId,
                Description = "High-end laptop",
                BatchNumber = "2-2026",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2",
                AddedBy = "System"
            

            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetDetailsByIdAsync(prodId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CategoryName, Is.EqualTo("Hardware"));
            Assert.That(result.SupplierName, Is.EqualTo("HP"));
            Assert.That(result.Description, Is.EqualTo("High-end laptop"));
        }

        [Test]
        public async Task ListCategory_ShouldReturnOrderedCategories()
        {
            // Arrange
            _context.Categories.AddRange(new List<Category>
            {
                new Category { CategoryId = Guid.NewGuid(), Name = "B" },
                new Category { CategoryId = Guid.NewGuid(), Name = "A" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.ListCategory();

            // Assert
            Assert.That(result[0].Name, Is.EqualTo("A"));
            Assert.That(result[1].Name, Is.EqualTo("B"));
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyProductFields()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Product { Id = id, Name = "Old Name", Price = 100,
                BatchNumber = "2-2026",
                Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2",
                AddedBy = "System"
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var editModel = new ProductEditViewModel
            {
                Id = id,
                Name = "New Name",
                Price = 150,
                StockQuantity = 20,

            };

            // Act
            await _service.UpdateAsync(editModel);

            // Assert
            var updated = await _context.Products.FindAsync(id);
            Assert.That(updated.Name, Is.EqualTo("New Name"));
            Assert.That(updated.Price, Is.EqualTo(150));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Products.Add(new Product
            {
                Id = id,
                Name = "To Delete",
                BatchNumber = "2-2026",
                AddedBy = "System",
                Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                ImageURL = "Folder/Folder1/image.jpeg",
                WarehouseLocationId = "a1-b2"
            });
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            var exists = await _context.Products.AnyAsync(p => p.Id == id);
            Assert.That(exists, Is.False);
        }
    }
}