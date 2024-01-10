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
    public class UserValidatorTests
    {
        [TestMethod]
        public async Task ValidateForAddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = "newuser",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyUsername_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = "",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User username cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongUsername_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = new string('f', 51),
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User username cannot be longer than 50 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithShortUsername_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = "f",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User username cannot be shorter than 5 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithExistingUsername_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = "Username1",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User with username Username1 already exists"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyPassword_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "hmads@gmail.com", Password = "", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User password cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongPassword_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "hmads@gmail.com", Password = new string('d', 51), ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User password cannot be longer than 50 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithShortPassword_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "hmads@gmail.com", Password = "d", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User password cannot be shorter than 5 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "", Email = "hmads@gmail.com", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User full name cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "d " + new string('4', 100), Email = "hmads@gmail.com", Password = "password123", ProfilePicture = Array.Empty<byte>() };
            
            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User full name cannot be longer than 100 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WitShortFullName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "d d", Email = "hmads@gmail.com", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"User full name cannot be shorter than 5 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithNotFullFulllName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "d", Email = "hmads@gmail.com", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User full name must contain first and last name"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyEmail_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);
            
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User email cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLongEmail_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);

            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = new string('d', 51), Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);
            
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"User email cannot be longer than 50 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithShortEmail_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);

            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "d", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);
            
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"User email cannot be shorter than 5 characters"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidFormatEmail_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);

            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "invalidemail", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);
            
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"User email is not in valid format"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithExistingEmail_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);

            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "email1@gmail.com", Password = "password123", ProfilePicture = Array.Empty<byte>() };

            // Act
            var result = await validator.ValidateForAddAsync(user);
            
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"User with email email1@gmail.com already exists"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithEmptyProfilePicture_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);

            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "email11@gmail.com", Password = "password123" };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User profile picture cannot be empty"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithLargeProfilePicture_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);

            var user = new UserDTO { Username = "newuser", FullName = "New User", Email = "email11@gmail.com", Password = "password123", ProfilePicture = new byte[10000001] };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User profile picture cannot be larger than 10MB"));
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithValidBookIds_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = "newuser",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>(),
                BookIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForAddAsync_WithInvalidBookIds_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Username = "newuser",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>(),
                BookIds = new List<int> { 1, 2, 3, 100 }
            };

            // Act
            var result = await validator.ValidateForAddAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("Book with id 100 does not exist"));
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Id = 1,
                Username = "newuser",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForUpdateAsync(user);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateForUpdateAsync_WithEmptyId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Id = default,
                Username = "newuser",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForUpdateAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User id cannot be empty"));
        }
        [TestMethod]
        public async Task ValidateForUpdateAsync_WithExistingName_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO
            {
                Id = 1,
                Username = "Username2",
                FullName = "New User",
                Email = "hmads@gmail.com",
                Password = "password123",
                ProfilePicture = Array.Empty<byte>()
            };

            // Act
            var result = await validator.ValidateForUpdateAsync(user);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains("User with username Username2 already exists"));
        }
        [TestMethod]
        public async Task ValidateIdAsync_WithValidId_ShouldReturnValidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Id = 1 };

            // Act
            var result = await validator.ValidateIdAsync(user.Id);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Messages.Any());
        }

        [TestMethod]
        public async Task ValidateIdAsync_WithInvalidId_ShouldReturnInvalidResult()
        {
            // Arrange
            var unitOfWork = TestUtilities.CreateUnitOfWork();
            var validator = new UserValidator(unitOfWork);
            var user = new UserDTO { Id = 11 };

            // Act
            var result = await validator.ValidateIdAsync(user.Id);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Messages.Any());
            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsTrue(result.Messages.Contains($"User with id {user.Id} does not exist"));
        }
    }
}
