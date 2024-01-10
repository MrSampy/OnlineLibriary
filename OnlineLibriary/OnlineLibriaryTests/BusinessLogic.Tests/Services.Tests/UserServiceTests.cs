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
    public class UserServiceTests
    {
        private UserService CreateService()
        {
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var mapper = TestUtilities.CreateMapper();
            var validator = new UserValidator(unitOfWork);
            var cacheService = TestUtilities.CreateCacheService();
            var hasher = TestUtilities.CreateSecurePasswordHasher();
            var service = new UserService(unitOfWork, mapper, cacheService, validator, hasher);
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
            Assert.AreEqual("Validation failed: User with id 0 does not exist", exception.Message);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var user = new UserDTO { Username = "Username11", Password = "Password1", FullName = "Fush Fush", Email = "mail@ggg.com", ProfilePicture = Array.Empty<byte>() };

            // Act
            await service.AddAsync(user);
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
            var user = new UserDTO { Username = "Username1", Password = "Password1", FullName = "Fush Fush", Email = "mail@ggg.com", ProfilePicture = Array.Empty<byte>() };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                  async () => await service.AddAsync(user));

            // Assert
            Assert.AreEqual("Validation failed: User with username Username1 already exists", exception.Message);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var service = CreateService();
            var user = new UserDTO {Id = 1, Username = "Username1", Password = "Password1", FullName = "Fush Fush", Email = "mail@ggg.com", ProfilePicture = Array.Empty<byte>() };

            // Act
            await service.UpdateAsync(user);
            var result = await service.GetByIdAsync(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.FullName, result.FullName);
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var service = CreateService();
            var user = new UserDTO { Id = 1, Username = "Username2", Password = "Password1", FullName = "Fush Fush", Email = "mail@ggg.com", ProfilePicture = Array.Empty<byte>() };

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                                 async () => await service.UpdateAsync(user));

            // Assert
            Assert.AreEqual("Validation failed: User with username Username2 already exists", exception.Message);
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
            var id = -100;

            // Act
            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(
                                                                 async () => await service.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Validation failed: User with id -100 does not exist", exception.Message);
        }

    }
}
