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
    public class GenreValidatorTests
    {
        [TestMethod]
        public async Task ValidateForAddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Name = "Genre11", BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForAddAsync(genre);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Name = "", BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForAddAsync(genre);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre name cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Name = new string('N', 51), BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForAddAsync(genre);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre name cannot be longer than 50 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithExistingName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Name = "Genre1", BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForAddAsync(genre);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre with name Genre1 already exists"));
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Id = 1, Name = "Genre1", BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForUpdateAsync(genre);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithEmptyId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Id = default, Name = "Genre1", BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForUpdateAsync(genre);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre id cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithExistingName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Id = 1, Name = "Genre2", BookIds = new List<int> { 1 } };

            // Act
            var result = await validator.ValidateForUpdateAsync(genre);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre with name Genre2 already exists"));
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithValidId_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Id = 1 };

            // Act
            var result = await validator.ValidateIdAsync(genre.Id);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithInvalidId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new GenreValidator(unitOfWork);
            var genre = new GenreDTO { Id = 11 };

            // Act
            var result = await validator.ValidateIdAsync(genre.Id);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Genre with id 11 does not exist"));
        }

    }
}
