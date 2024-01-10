using BusinessLogic.Models.DTOs;
using BusinessLogic.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriary.Tests.BusinessLogic.Tests.Validation.Tests
{
    [TestClass]
    public class BookValidatorTests
    {
        [TestMethod]
        public async Task ValidateForAddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = "Book1", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 1 };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyTitle_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = "", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 1 };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book title cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongTitle_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = new string('T', 101), Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 1 };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book title cannot be longer than 100 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithShortTitle_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = new string('T', 1), Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 1 };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book title cannot be shorter than 1 character"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidAuthorId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = "Book1", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 100, GenreId = 1 };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author with id 100 does not exist"));
        }


        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyGenreId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = "Book1", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = default };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre id cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidGenreId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Title = "Book1", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 100 };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre with id 100 does not exist"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyYear_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                BookContent = Array.Empty<byte>(),
                Year = default,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Publication year cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidYear_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                BookContent = Array.Empty<byte>(),
                Year = 10000,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Publication year cannot be greater than current year"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithNegativeYear_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                BookContent = Array.Empty<byte>(),
                Year = -100,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Publication year cannot be negative"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyDescription_ShouldReturnInvaliddResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "",
                BookContent = Array.Empty<byte>(),
                Year = 2021,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book description cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongDescription_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var description = new string('D', 1001);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = description,
                BookContent = Array.Empty<byte>(),
                Year = 2021,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book description cannot be longer than 1000 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithShortDescription_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var description = new string('D', 1);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = description,
                BookContent = Array.Empty<byte>(),
                Year = 2021,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book description cannot be shorter than 10 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyBookContent_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                Year = 2021,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book content cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongBookContent_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var bookContent = new byte[1000001];
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                BookContent = bookContent,
                Year = 2021,
                AuthorId = 1,
                GenreId = 1
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book content cannot be longer than 1MB"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithValidUserIds_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var userIds = new List<int> { 1, 2, 3 };
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                BookContent = Array.Empty<byte>(),
                Year = 2021,
                AuthorId = 1,
                GenreId = 1,
                UserIds = userIds
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidUserIds_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var userIds = new List<int> { 1, 2, 3, 100 };
            var book = new BookDTO
            {
                Title = "Book1",
                Description = "Description1",
                BookContent = Array.Empty<byte>(),
                Year = 2021,
                AuthorId = 1,
                GenreId = 1,
                UserIds = userIds
            };

            // Act
            var result = await validator.ValidateForAddAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User with id 100 does not exist"));
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Id = 1, Title = "Book1", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 1 };

            // Act
            var result = await validator.ValidateForUpdateAsync(book);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithInvalidId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var book = new BookDTO { Id = 100, Title = "Book1", Description = "Description1", BookContent = Array.Empty<byte>(), Year = 2021, AuthorId = 1, GenreId = 1 };

            // Act
            var result = await validator.ValidateForUpdateAsync(book);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book with id 100 does not exist"));
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithValidId_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var id = 1;

            // Act
            var result = await validator.ValidateIdAsync(id);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithInvalidId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new BookValidator(unitOfWork);
            var id = 100;

            // Act
            var result = await validator.ValidateIdAsync(id);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book with id 100 does not exist"));
        }
    }
}
