using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.DTOs;

namespace OnlineLibriary.Tests.BusinessLogic.Tests.Services.Tests
{
    [TestClass]
    public class GenreServiceTests
    {
        private GenreService CrreateService()
        {
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var mapper = TestUtilities.CreateMapper();
            var validator = new GenreValidator(unitOfWork);
            var cacheService = TestUtilities.CreateCacheService();
            var service = new GenreService(unitOfWork, mapper, cacheService, validator);
            return service;
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CrreateService();
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
            var service = CrreateService();
            var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 5 };

            // Act
            var result = await service.GetAllAsync(paginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public async Task GetAllAsync_WithInvalidPagination_ShouldReturnValidResult()
        {
            // Arrange
            var service = CrreateService();
            var paginationModel = new PaginationModel { PageNumber = 0, PageSize = 0 };

            // Act
            var result = await service.GetAllAsync(paginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CrreateService();
            var id = 1;

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidData_ShouldThorwException()
        {
            // Arrange
            var service = CrreateService();
            var id = 100;

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                    async () => await service.GetByIdAsync(id));

            // Assert
            Assert.AreEqual("Validation failed: Genre with id 100 does not exist", exception.Message);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CrreateService();
            var genre = new GenreDTO { Name = "Genre11", BookIds = new List<int> { 1 } };

            // Act
            await service.AddAsync(genre);
            var result = await service.GetAllAsync(new PaginationModel { });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(11, result.Count());
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldThorwException()
        {
            // Arrange
            var service = CrreateService();
            var genre = new GenreDTO { Name = "Genre1", BookIds = new List<int> { 1 } };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                   async () => await service.AddAsync(genre));

            // Assert
            Assert.AreEqual("Validation failed: Genre with name Genre1 already exists", exception.Message);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CrreateService();
            var genre = new GenreDTO { Id = 1, Name = "Genre11", BookIds = new List<int> { 1 } };

            // Act
            await service.UpdateAsync(genre);
            var result = await service.GetByIdAsync(genre.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(genre.Id, result.Id);
            Assert.AreEqual(genre.Name, result.Name);
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldThorwException()
        {
            // Arrange
            var service = CrreateService();
            var genre = new GenreDTO { Id = 100, Name = "Genre11", BookIds = new List<int> { 1 } };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                  async () => await service.UpdateAsync(genre));

            // Assert
            Assert.AreEqual("Validation failed: Genre with id 100 does not exist", exception.Message);
        }

        [TestMethod]
        public async Task DeleteAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CrreateService();
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
        public async Task DeleteAsync_WithInvalidData_ShouldThorwException()
        {
            // Arrange
            var service = CrreateService();
            var id = -100;

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                                 async () => await service.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Validation failed: Genre with id -100 does not exist", exception.Message);
        }
    }
}
