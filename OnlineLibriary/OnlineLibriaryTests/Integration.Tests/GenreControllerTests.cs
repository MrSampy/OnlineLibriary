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
    public class GenreControllerTests
    {
        private readonly APIBuilder _apiBuilder;
        public GenreControllerTests()
        {
            _apiBuilder = new APIBuilder();
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            List<Genre> expectedGenres = TestUtilities.CreateGenres();
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            var actualGenresDTOs = await _apiBuilder.GetRequestWithDeserializing<List<GenreDTO>>("api/genre");
            var actualGenres = actualGenresDTOs.Select(g => new Genre { Id = g.Id, Name = g.Name }).ToList();

            // Assert
            Assert.IsNotNull(actualGenres);
            Assert.AreEqual(expectedGenres.Count, actualGenres.Count);
            Assert.IsTrue(expectedGenres.SequenceEqual(actualGenres, new GenreEqualityComparer()));
        }

        [TestMethod]
        public async Task GetAllAsync_WithPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            List<Genre> expectedGenres = TestUtilities.CreateGenres();
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 5 };

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            var actualGenresDTOs = await _apiBuilder.GetRequestWithDeserializing<List<GenreDTO>>($"api/genre?pageNumber={paginationModel.PageNumber}&pageSize={paginationModel.PageSize}");
            var actualGenres = actualGenresDTOs.Select(g => new Genre { Id = g.Id, Name = g.Name }).ToList();

            // Assert
            Assert.IsNotNull(actualGenres);
            Assert.AreEqual(paginationModel.PageSize, actualGenres.Count);
            Assert.IsTrue(expectedGenres.Take(paginationModel.PageSize.Value).SequenceEqual(actualGenres, new GenreEqualityComparer()));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            List<Genre> expectedGenres = TestUtilities.CreateGenres();
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var expectedGenre = expectedGenres.First();

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            var actualGenreDTO = await _apiBuilder.GetRequestWithDeserializing<GenreDTO>($"api/genre/{expectedGenre.Id}");

            // Assert
            Assert.IsNotNull(actualGenreDTO);
            Assert.AreEqual(expectedGenre.Id, actualGenreDTO.Id);
            Assert.AreEqual(expectedGenre.Name, actualGenreDTO.Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var expectedMessage = "Validation failed: Genre with id -1 does not exist";
            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            var response = await _apiBuilder.GetRequest($"api/genre/-1");

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedMessage, response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var newGenre = new GenreDTO { Id = 11, Name = "New Genre", BookIds = new List<int> { 2 }  };
            var expectedGenre = new Genre { Id = 11, Name = "New Genre" };

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            await _apiBuilder.PostRequestWithDeserializing<GenreDTO>("api/genre", newGenre);
            var actualGenreDTO = await _apiBuilder.GetRequestWithDeserializing<GenreDTO>($"api/genre/{expectedGenre.Id}");

            // Assert
            Assert.IsNotNull(actualGenreDTO);
            Assert.AreEqual(expectedGenre.Id, actualGenreDTO.Id);
            Assert.AreEqual(expectedGenre.Name, actualGenreDTO.Name);
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var newGenre = new GenreDTO { Name = "Genre1", BookIds = new List<int> { 2 } };
            var expectedMessage = "Validation failed: Genre with name Genre1 already exists";
            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            var response = await _apiBuilder.PostRequest("api/genre", newGenre);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedMessage, response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var updatedGenre = new GenreDTO { Id = 1, Name = "Updated Genre", BookIds = new List<int> { 2 } };
            var expectedGenre = new Genre { Id = 1, Name = "Updated Genre" };

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            await _apiBuilder.PutRequest("api/genre", updatedGenre);
            var actualGenreDTO = await _apiBuilder.GetRequestWithDeserializing<GenreDTO>($"api/genre/{expectedGenre.Id}");

            // Assert
            Assert.IsNotNull(actualGenreDTO);
            Assert.AreEqual(expectedGenre.Id, actualGenreDTO.Id);
            Assert.AreEqual(expectedGenre.Name, actualGenreDTO.Name);
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var updatedGenre = new GenreDTO { Id = 11, Name = "Updated Genre", BookIds = new List<int> { 2 } };
            var expectedMessage = "Validation failed: Genre with id 11 does not exist";
            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);
            var response = await _apiBuilder.PutRequest("api/genre", updatedGenre);

            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedMessage, response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public async Task DeleteByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var expectedGenre = new Genre { Id = 1, Name = "Genre1" };
            var expectedMessage = "Validation failed: Genre with id 1 does not exist";

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);            
            await _apiBuilder.DeleteRequest($"api/genre/{expectedGenre.Id}");
            var response = await _apiBuilder.GetRequest($"api/genre/{expectedGenre.Id}");

            // Assert
            Assert.AreEqual(expectedMessage, response.Content.ReadAsStringAsync().Result);
        }

        [TestMethod]
        public async Task DeleteByIdAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var signInModel = new UserCredentials { Username = "Username1", Password = "Password1" };
            var expectedGenre = new Genre { Id = 11, Name = "Genre1" };
            var expectedMessage = "Validation failed: Genre with id 11 does not exist";

            // Act
            var token = (await _apiBuilder.PostRequestWithDeserializing<Token>("api/signin", signInModel)).AccessToken;
            _apiBuilder.SetAuthorizationToken(token);            
            var response = await _apiBuilder.DeleteRequest($"api/genre/{expectedGenre.Id}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(expectedMessage, response.Content.ReadAsStringAsync().Result);
        }
    }
}
