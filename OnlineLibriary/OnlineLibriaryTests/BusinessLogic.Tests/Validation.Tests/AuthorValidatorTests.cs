using BusinessLogic.Models.DTOs;
using BusinessLogic.Validation;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriary.Tests.BusinessLogic.Tests.Validation.Tests
{
    [TestClass]
    public class AuthorValidatorTests
    {
        [TestMethod]
        public async Task ValidateForAddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "Author1 Author1", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2} };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author full name cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithShortFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "A A", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author full name cannot be shorter than 5 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = new string('A', 100) + " A", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author full name cannot be longer than 100 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "A", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author full name must contain first and last name"));
        }

        public async Task ValidateForAddAsync_WithEmptyDateOfBirth_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "Author1 Author1", DateOfBirth = default, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author date of birth cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidDateOfBirth_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "Author1 Author1", DateOfBirth = DateTime.Now.AddDays(1), Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author date of birth cannot be in the future"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyCountry_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "Author1 Author1", DateOfBirth = DateTime.Now, Country = "", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author country cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithValidBookIds_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var book = new Book
            {
                Title = "New Book",
                Description = "Description of the new book",
                Year = 2023,
                GenreId = 1,
                BookContent = new byte[0]
            };
            await unitOfWork.BookRepository.AddAsync(book);
            await unitOfWork.SaveAsync();
            var author = new AuthorDTO { FullName = "Author1 Author1", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 11 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidBookIds_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { FullName = "Author1 Author1", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 111 } };

            // Act
            var result = await validator.ValidateForAddAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"Book with id {author.BookIds.First()} does not exist"));
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { Id = 1, FullName = "Author1 Author1", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForUpdateAsync(author);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithEmptyId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { Id = default, FullName = "Author1 Author1", DateOfBirth = DateTime.Now, Country = "Country", BookIds = new List<int> { 1, 2 } };

            // Act
            var result = await validator.ValidateForUpdateAsync(author);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Author id cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithValidId_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { Id = 1 };

            // Act
            var result = await validator.ValidateIdAsync(author.Id);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithInvalidId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new AuthorValidator(unitOfWork);
            var author = new AuthorDTO { Id = 11 };

            // Act
            var result = await validator.ValidateIdAsync(author.Id);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"Author with id {author.Id} does not exist"));
        }

    }
}
