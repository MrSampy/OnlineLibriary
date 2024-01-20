using API.Models;
using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using Data.Entities;
using OnlineLibriary.Tests.Utils;
using OnlineLibriaryTests.Utils.Compareres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriary.Tests.Integration.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private readonly APIBuilder _apiBuilder;
        public UserControllerTests()
        {
            _apiBuilder = new APIBuilder();
        }

        [TestInitialize]
        public async Task Initialize()
        {
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", new UserCredentials { Username = "Username1", Password = "Password1" })).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
        }

        [TestMethod]
        public async Task GetAllAsync_WithPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateUsers();
            var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 5 };

            // Act
            var actualDTOs = await _apiBuilder.GetRequestWithDeserializing<List<UserDTO>>($"api/user?pageNumber={paginationModel.PageNumber}&pageSize={paginationModel.PageSize}");
            var actual = actualDTOs.Select(g => new User { Id = g.Id, Username = g.Username, Email = g.Email, FirstName = g.FullName.Split().First(), SurName = g.FullName.Split().Last(), Password = g.Password, ProfilePicture = g.ProfilePicture }).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(paginationModel.PageSize, actual.Count);
            Assert.IsTrue(expected.Take(paginationModel.PageSize.Value).SequenceEqual(actual, new UserEqualityComparer()));
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateUsers();
            var paginationModel = new PaginationModel { PageNumber = null, PageSize = null };

            // Act
            var actualDTOs = await _apiBuilder.GetRequestWithDeserializing<List<UserDTO>>($"api/user");
            var actual = actualDTOs.Select(g => new User { Id = g.Id, Username = g.Username, Email = g.Email, FirstName = g.FullName.Split().First(), SurName = g.FullName.Split().Last(), Password = g.Password, ProfilePicture = g.ProfilePicture }).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.IsTrue(expected.SequenceEqual(actual, new UserEqualityComparer()));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateUsers().First();

            // Act
            var actualDTO = await _apiBuilder.GetRequestWithDeserializing<UserDTO>($"api/user/{expected.Id}");
            var actual = new User { Id = actualDTO.Id, Username = actualDTO.Username, Email = actualDTO.Email, FirstName = actualDTO.FullName.Split().First(), SurName = actualDTO.FullName.Split().Last(), Password = actualDTO.Password, ProfilePicture = actualDTO.ProfilePicture };

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new UserEqualityComparer().Equals(expected, actual));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: User with id 100 does not exist";
            var invalidId = 100;

            // Act
            var response = await _apiBuilder.GetRequest($"api/user/{invalidId}");
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = new User { Id = 11, FirstName = "FirstName4", SurName = "SurName4", Username = "UserName", Email = "mail@gm.com", ProfilePicture = new byte[0], Password = "Password" };
            var userDTO = new UserDTO { Id = 11, BookIds = new List<int> { 3 }, FullName = "FirstName4 SurName4", Username = "UserName", Email = "mail@gm.com", ProfilePicture = new byte[0], Password = "Password" };

            // Act
            await _apiBuilder.PostRequestWithDeserializing<UserDTO>($"api/user", userDTO);
            var actual = await _apiBuilder.GetRequestWithDeserializing<UserDTO>($"api/user/{expected.Id}");

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new UserEqualityComparer().Equals(actual, expected));
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: User full name must contain first and last name, User email is not in valid format";
            var userDTO = new UserDTO { Id = 11, BookIds = new List<int> { 3 }, FullName = "SurName4", Username = "UserName", Email = "dgdfhgd.com", ProfilePicture = new byte[0], Password = "Password" };

            // Act
            var response = await _apiBuilder.PostRequest("api/user", userDTO);
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = new User { Id = 10, FirstName = "FirstName4", SurName = "SurName4", Username = "UserName", Email = "mail@gm.com", ProfilePicture = new byte[0], Password = "Password" };
            var userDTO = new UserDTO { Id = 10, BookIds = new List<int> { 3 }, FullName = "FirstName4 SurName4", Username = "UserName", Email = "mail@gm.com", ProfilePicture = new byte[0], Password = "Password" };

            // Act
            await _apiBuilder.PutRequestWithDeserializing<UserDTO>($"api/user", userDTO);
            var actual = await _apiBuilder.GetRequestWithDeserializing<UserDTO>($"api/user/{expected.Id}");

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new UserEqualityComparer().Equals(actual, expected));
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: User email cannot be shorter than 5 characters";
            var userDTO = new UserDTO { Id = 10, BookIds = new List<int> { 3 }, FullName = "FirstName4 SurName4", Username = "UserName1", Email = ".com", ProfilePicture = new byte[0], Password = "Password" };

            // Act
            var response = await _apiBuilder.PutRequest("api/user", userDTO);
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task DeleteAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var deleteId = 1;
            var expectedMessage = "Validation failed: User with id 1 does not exist";

            // Act
            await _apiBuilder.DeleteRequest($"api/user/{deleteId}");
            var response = await _apiBuilder.GetRequest($"api/user/{deleteId}");
            var actualMessage = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task DeleteAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var deleteId = 100;
            var expectedMessage = "Validation failed: User with id 100 does not exist";

            // Act
            var response = await _apiBuilder.DeleteRequest($"api/user/{deleteId}");
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
