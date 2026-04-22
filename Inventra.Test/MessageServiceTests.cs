using Inventra.Core.Services;
using Inventra.Core.ViewModels.Messages;
using Inventra.Data;
using Inventra.Data.Entities;
using Inventra.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inventra.Tests
{
    [TestFixture]
    public class MessageServiceTests
    {
        private InventraDbContext _context;
        private MessageService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventraDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InventraDbContext(options);
            _service = new MessageService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldSaveMessageCorrectly()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var model = new MessageCreateViewModel
            {
                Id = messageId,
                Content = "Системно съобщение",
                CreatedBy = "Admin",
                Type = MessageType.Info
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var savedMessage = await _context.Messages.FindAsync(messageId);
            Assert.That(savedMessage, Is.Not.Null);
            Assert.That(savedMessage.Content, Is.EqualTo("Системно съобщение"));
            Assert.That(savedMessage.Type, Is.EqualTo(MessageType.Info));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllMessagesWithCorrectData()
        {
            // Arrange
            var now = DateTime.Now;
            _context.Messages.AddRange(new List<Message>
            {
                new Message { Id = Guid.NewGuid(), Content = "M1", CreatedBy = "A", Type = MessageType.Info, CreatedAt = now },
                new Message { Id = Guid.NewGuid(), Content = "M2", CreatedBy = "B", Type = MessageType.Crucial, CreatedAt = now.AddHours(1) }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(m => m.Content == "M1"), Is.True);
            Assert.That(result.Any(m => m.Type == MessageType.Crucial), Is.True);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveMessageIfExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _context.Messages.Add(new Message
            {
                Id = id,
                Content = "За триене",
                CreatedBy = "System",
                Type = MessageType.Info
            });
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            var exists = await _context.Messages.AnyAsync(m => m.Id == id);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task DeleteAsync_WithInvalidId_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _service.DeleteAsync(Guid.NewGuid()));
        }
    }
}