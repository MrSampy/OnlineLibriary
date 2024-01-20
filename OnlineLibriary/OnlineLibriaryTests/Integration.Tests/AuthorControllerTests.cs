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
    public class AuthorControllerTests
    {
        private readonly APIBuilder _apiBuilder;
        public AuthorControllerTests()
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
            var expected = TestUtilities.CreateAuthors();
            var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 5 };

            // Act
            var actualDTOs = await _apiBuilder.GetRequestWithDeserializing<List<AuthorDTO>>($"api/author?pageNumber={paginationModel.PageNumber}&pageSize={paginationModel.PageSize}");
            var actual = actualDTOs.Select(g => new Author { Id = g.Id, FirstName = g.FullName.Split().First(), SurName = g.FullName.Split().Last(), Country = g.Country, DateOfBirth = g.DateOfBirth }).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(paginationModel.PageSize, actual.Count);
            Assert.IsTrue(expected.Take(paginationModel.PageSize.Value).SequenceEqual(actual, new AuthorEqualityComparer()));
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateAuthors();
            var paginationModel = new PaginationModel { PageNumber = null, PageSize = null };

            // Act
            var actualDTOs = await _apiBuilder.GetRequestWithDeserializing<List<AuthorDTO>>($"api/author");
            var actual = actualDTOs.Select(g => new Author { Id = g.Id, FirstName = g.FullName.Split().First(), SurName = g.FullName.Split().Last(), Country = g.Country, DateOfBirth = g.DateOfBirth }).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.SequenceEqual(actual, new AuthorEqualityComparer()));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateAuthors().First();

            // Act
            var actualDTO = await _apiBuilder.GetRequestWithDeserializing<AuthorDTO>($"api/author/{expected.Id}");
            var actual = new Author { Id = actualDTO.Id, FirstName = actualDTO.FullName.Split().First(), SurName = actualDTO.FullName.Split().Last(), Country = actualDTO.Country, DateOfBirth = actualDTO.DateOfBirth };

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new AuthorEqualityComparer().Equals(expected, actual));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: Author with id 100 does not exist";
            var invalidId = 100;

            // Act
            var response = await _apiBuilder.GetRequest($"api/author/{invalidId}");
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = new Author { Id = 11, FirstName = "FirstName4", SurName = "SurName4", Country = "Country4", DateOfBirth = new DateTime(2000, 1, 1) };
            var authorDTO = new AuthorDTO { BookIds = new List<int> { 3}, FullName = "FirstName4 SurName4", Country = "Country4", DateOfBirth = new DateTime(2000, 1, 1) };

            // Act
            await _apiBuilder.PostRequestWithDeserializing<AuthorDTO>($"api/author", authorDTO);
            var actual = await _apiBuilder.GetRequestWithDeserializing<AuthorDTO>($"api/author/{expected.Id}");

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new AuthorEqualityComparer().Equals(actual, expected));
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: Author full name must contain first and last name, Book with id 13 does not exist";
            var authorDTO = new AuthorDTO { BookIds = new List<int> { 13 }, FullName = "FirstName4", Country = "Country4", DateOfBirth = new DateTime(2000, 1, 1) };

            // Act
            var response = await _apiBuilder.PostRequest("api/author", authorDTO);
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = new Author { Id = 1, FirstName = "FirstName4", SurName = "SurName4", Country = "Country4", DateOfBirth = new DateTime(2000, 1, 1) };
            var authorDTO = new AuthorDTO { Id = 1, BookIds = new List<int> { 3 }, FullName = "FirstName4 SurName4", Country = "Country4", DateOfBirth = new DateTime(2000, 1, 1) };

            // Act
            await _apiBuilder.PutRequestWithDeserializing<AuthorDTO>($"api/author", authorDTO);
            var actual = await _apiBuilder.GetRequestWithDeserializing<AuthorDTO>($"api/author/{expected.Id}");

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new AuthorEqualityComparer().Equals(actual, expected));
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: Author full name must contain first and last name, Book with id 13 does not exist";
            var authorDTO = new AuthorDTO { Id = 1, BookIds = new List<int> { 13 }, FullName = "FirstName4", Country = "Country4", DateOfBirth = new DateTime(2000, 1, 1) };

            // Act
            var response = await _apiBuilder.PutRequest("api/author", authorDTO);
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task DeleteAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var deleteId = 1;
            var expectedMessage = "Validation failed: Author with id 1 does not exist";

            // Act
            await _apiBuilder.DeleteRequest($"api/author/{deleteId}");
            var response = await _apiBuilder.GetRequest($"api/author/{deleteId}");
            var actualMessage = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task DeleteAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var deleteId = 100;
            var expectedMessage = "Validation failed: Author with id 100 does not exist";

            // Act
            var response = await _apiBuilder.DeleteRequest($"api/author/{deleteId}");
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
