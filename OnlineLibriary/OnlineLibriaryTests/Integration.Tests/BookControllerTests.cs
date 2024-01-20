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
    public class BookControllerTests
    {
        private readonly APIBuilder _apiBuilder;
        public BookControllerTests()
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
            var expected = TestUtilities.CreateBooks();
            var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 5 };

            // Act
            var actualDTOs = await _apiBuilder.GetRequestWithDeserializing<List<BookDTO>>($"api/book?pageNumber={paginationModel.PageNumber}&pageSize={paginationModel.PageSize}");
            var actual = actualDTOs.Select(g => new Book { Id = g.Id, Title = g.Title, Description = g.Description, AuthorId = g.AuthorId, GenreId = g.GenreId, BookContent = g.BookContent, Year = g.Year }).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(paginationModel.PageSize, actual.Count);
            Assert.IsTrue(expected.Take(paginationModel.PageSize.Value).SequenceEqual(actual, new BookEqualityComparer()));
        }

        [TestMethod]
        public async Task GetAllAsync_WithNoPaginationValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateBooks();
            var paginationModel = new PaginationModel { PageNumber = null, PageSize = null };

            // Act
            var actualDTOs = await _apiBuilder.GetRequestWithDeserializing<List<BookDTO>>($"api/book");
            var actual = actualDTOs.Select(g => new Book { Id = g.Id, Title = g.Title, Description = g.Description, AuthorId = g.AuthorId, GenreId = g.GenreId, BookContent = g.BookContent, Year = g.Year }).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.SequenceEqual(actual, new BookEqualityComparer()));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = TestUtilities.CreateBooks().First();

            // Act
            var actualDTO = await _apiBuilder.GetRequestWithDeserializing<BookDTO>($"api/book/{expected.Id}");
            var actual = new Book { Id = actualDTO.Id, Title = actualDTO.Title, Description = actualDTO.Description, AuthorId = actualDTO.AuthorId, GenreId = actualDTO.GenreId, BookContent = actualDTO.BookContent, Year = actualDTO.Year };

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new BookEqualityComparer().Equals(expected, actual));
        }

        [TestMethod]
        public async Task GetByIdAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var id = 100;
            var expectedMessage = "Validation failed: Book with id 100 does not exist";

            // Act
            var response = await _apiBuilder.GetRequest($"api/book/{id}");
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = new Book {Id = 11, Title = "New Book", Description = "New Description", AuthorId = 1, GenreId = 1, BookContent = new byte[0], Year = 2020 };
            var expectedDTO = new BookDTO { Title = "New Book", Description = "New Description", AuthorId = 1, GenreId = 1, BookContent = new byte[0], Year = 2020, UserIds = new List<int>() };
            // Act
            await _apiBuilder.PostRequestWithDeserializing<BookDTO>("api/book", expectedDTO);
            var actualDTO = await _apiBuilder.GetRequestWithDeserializing<BookDTO>($"api/book/{expected.Id}");
            var actual = new Book { Id = actualDTO.Id, Title = actualDTO.Title, Description = actualDTO.Description, AuthorId = actualDTO.AuthorId, GenreId = actualDTO.GenreId, BookContent = actualDTO.BookContent, Year = actualDTO.Year };

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new BookEqualityComparer().Equals(expected, actual));
        }

        [TestMethod]
        public async Task AddAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: Author with id 11 does not exist, Book description cannot be shorter than 10 characters";
            var bookDTO = new BookDTO { Title = "New Book", Description = "New", AuthorId = 11, GenreId = 1, BookContent = new byte[0], Year = 2020, UserIds = new List<int>() };

            // Act
            var response = await _apiBuilder.PostRequest("api/book", bookDTO);
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var expected = new Book { Id = 1, Title = "New Book", Description = "New Description", AuthorId = 1, GenreId = 1, BookContent = new byte[0], Year = 2020 };
            var expectedDTO = new BookDTO { Id = 1, Title = "New Book", Description = "New Description", AuthorId = 1, GenreId = 1, BookContent = new byte[0], Year = 2020, UserIds = new List<int>() };

            // Act
            await _apiBuilder.PutRequestWithDeserializing<BookDTO>("api/book", expectedDTO);
            var actualDTO = await _apiBuilder.GetRequestWithDeserializing<BookDTO>($"api/book/{expected.Id}");
            var actual = new Book { Id = actualDTO.Id, Title = actualDTO.Title, Description = actualDTO.Description, AuthorId = actualDTO.AuthorId, GenreId = actualDTO.GenreId, BookContent = actualDTO.BookContent, Year = actualDTO.Year };

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(new BookEqualityComparer().Equals(expected, actual));
        }

        [TestMethod]
        public async Task UpdateAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var expectedMessage = "Validation failed: Author with id 11 does not exist, Book description cannot be shorter than 10 characters";
            var bookDTO = new BookDTO { Id = 1, Title = "New Book", Description = "New", AuthorId = 11, GenreId = 1, BookContent = new byte[0], Year = 2020, UserIds = new List<int>() };

            // Act
            var response = await _apiBuilder.PutRequest("api/book", bookDTO);
            var actualMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task DeleteAsync_WithValidData_ShouldReturnValidResult()
        {
            // Arrange
            var deleteId = 1;
            var expectedMessage = "Validation failed: Book with id 1 does not exist";

            // Act
            await _apiBuilder.DeleteRequest($"api/book/{deleteId}");
            var response = await _apiBuilder.GetRequest($"api/book/{deleteId}");
            var actualMessage = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public async Task DeleteAsync_WithInvalidData_ShouldReturnValidResult()
        {
            // Arrange
            var deleteId = 100;
            var expectedMessage = "Validation failed: Book with id 100 does not exist";

            // Act
            var response = await _apiBuilder.DeleteRequest($"api/book/{deleteId}");
            var actualMessage = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
