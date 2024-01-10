using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using BusinessLogic.Services;
using BusinessLogic.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriary.Tests.BusinessLogic.Tests.Services.Tests
{
    [TestClass]
    public class BookServiceTests
    {
        private BookService CreateService()
        {
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var mapper = TestUtilities.CreateMapper();
            var validator = new BookValidator(unitOfWork);
            var cacheService = TestUtilities.CreateCacheService();
            var service = new BookService(unitOfWork, mapper, cacheService, validator);
            return service;
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var paginationModel = new PaginationModel { };

            // Act
            var result = await service.GetAllAsync(paginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        public async Task GetAllAsync_WithPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 5 };

            // Act
            var result = await service.GetAllAsync(paginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var id = 1;

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var id = 0;

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                   async () => await service.GetByIdAsync(id));

            // Assert
            Assert.AreEqual("Validation failed: Book with id 0 does not exist", exception.Message);
        }

        [TestMethod]
        public async Task AddAsyncc_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var bookModel = new BookDTO
            {
                Title = "Title",
                Description = "Description",
                AuthorId = 1,
                GenreId = 1,
                BookContent = new byte[0],
                Year = 2014
            };

            // Act
            await service.AddAsync(bookModel);
            var result = await service.GetByIdAsync(11);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookModel.Title, result.Title);
            Assert.AreEqual(bookModel.Description, result.Description);
            Assert.AreEqual(bookModel.AuthorId, result.AuthorId);
            Assert.AreEqual(bookModel.GenreId, result.GenreId);
            Assert.AreEqual(bookModel.Year, result.Year);
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var bookModel = new BookDTO
            {
                Title = "",
                Description = "Description",
                AuthorId = 1,
                GenreId = 1,
                BookContent = new byte[0],
                Year = 2014
            };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                  async () => await service.AddAsync(bookModel));

            // Assert
            Assert.AreEqual("Validation failed: Book title cannot be empty", exception.Message);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var bookModel = new BookDTO
            {
                Id = 1,
                Title = "Title",
                Description = "Description",
                AuthorId = 1,
                GenreId = 1,
                BookContent = new byte[0],
                Year = 2014
            };

            // Act
            await service.UpdateAsync(bookModel);
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookModel.Title, result.Title);
            Assert.AreEqual(bookModel.Description, result.Description);
            Assert.AreEqual(bookModel.AuthorId, result.AuthorId);
            Assert.AreEqual(bookModel.GenreId, result.GenreId);
            Assert.AreEqual(bookModel.Year, result.Year);
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var bookModel = new BookDTO
            {
                Id = -1,
                Title = "Title",
                Description = "Description",
                AuthorId = 1,
                GenreId = 1,
                BookContent = new byte[0],
                Year = 2014
            };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                                 async () => await service.UpdateAsync(bookModel));

            // Assert
            Assert.AreEqual("Validation failed: Book with id -1 does not exist", exception.Message);
        }

        [TestMethod]
        public async Task DeleteAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var id = 1;

            // Act
            await service.DeleteAsync(id);
            var result = await service.GetAllAsync(new PaginationModel { });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(9, result.Count());
        }

        [TestMethod]
        public async Task DeleteAsync_WithInValidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var id = -1;

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                                                async () => await service.GetByIdAsync(id));

            // Assert
            Assert.AreEqual("Validation failed: Book with id -1 does not exist", exception.Message);
        }
    }
}
