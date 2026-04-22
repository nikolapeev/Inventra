using Inventra.Core.Services;
using Inventra.Core.ViewModels.Categories;
using Inventra.Data;
using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private InventraDbContext _context;
        private CategoryService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new CategoryService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var model = new CategoryCreateViewModel { Name = "Електроника" };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var category = await _context.Categories.FirstOrDefaultAsync();
            Assert.That(category, Is.Not.Null);
            Assert.That(category.Name, Is.EqualTo("Електроника"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnCategoriesSortedByName()
        {
            // Arrange
            _context.Categories.AddRange(new List<Category>
            {
                new Category { CategoryId = Guid.NewGuid(), Name = "В" },
                new Category { CategoryId = Guid.NewGuid(), Name = "А" },
                new Category { CategoryId = Guid.NewGuid(), Name = "Б" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("А"));
            Assert.That(result[2].Name, Is.EqualTo("В"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectCategory()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Categories.Add(new Category { CategoryId = id, Name = "Тест" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Тест"));
            Assert.That(result.CategoryId, Is.EqualTo(id));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveCategoryWhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Categories.Add(new Category { CategoryId = id, Name = "За триене" });
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            var exists = await _context.Categories.AnyAsync(x => x.CategoryId == id);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task UpdateAsync_ShouldChangeNameCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = new Category { CategoryId = id, Name = "Старо име" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var updateModel = new CategoryIndexViewModel { CategoryId = id, Name = "Ново име" };

            // Act
            await _service.UpdateAsync(updateModel);

            // Assert
            var updatedCat = await _context.Categories.FindAsync(id);
            Assert.That(updatedCat.Name, Is.EqualTo("Ново име"));
        }
    }
}