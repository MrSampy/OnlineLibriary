using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using BusinessLogic.Services;
using BusinessLogic.Validation;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriary.Tests.BusinessLogic.Tests.Services.Tests
{
    [TestClass]
    public class AuthorServiceTests
    {
        private AuthorService CreateService() 
        {
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var mapper = TestUtilities.CreateMapper();
            var validator = new AuthorValidator(unitOfWork);
            var cacheService = TestUtilities.CreateCacheService();
            var service = new AuthorService(unitOfWork, mapper, cacheService, validator);
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
            Assert.AreEqual("Validation failed: Author with id 0 does not exist", exception.Message);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var model = new AuthorDTO { FullName = "Name1 Surname1", DateOfBirth = DateTime.Today, Country = "Country" };

            // Act
            await service.AddAsync(model);
            var result = await service.GetAllAsync(new PaginationModel { });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(11, result.Count());
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var model = new AuthorDTO { FullName = "Name1 Surname1", DateOfBirth = DateTime.Today.AddDays(1), Country = "Country" };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                   async () => await service.AddAsync(model));

            // Assert
            Assert.AreEqual("Validation failed: Author date of birth cannot be in the future", exception.Message);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var model = new AuthorDTO { Id = 1, FullName = "Name111 Surname1", DateOfBirth = DateTime.Today, Country = "Country" };

            // Act
            await service.UpdateAsync(model);
            var result = await service.GetByIdAsync(model.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.FullName, result.FullName);
            Assert.AreEqual(model.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(model.Country, result.Country);
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var model = new AuthorDTO { Id = -1, FullName = "Name1 Surname1", DateOfBirth = DateTime.Today.AddDays(1), Country = "Country" };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                  async () => await service.UpdateAsync(model));

            // Assert
            Assert.AreEqual("Validation failed: Author with id -1 does not exist, Author date of birth cannot be in the future", exception.Message);
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
        public async Task DeleteAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var id = -1;

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                                 async () => await service.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Validation failed: Author with id -1 does not exist", exception.Message);
        }
    }
}
