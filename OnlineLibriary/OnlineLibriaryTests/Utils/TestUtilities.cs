using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Utils;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriaryTests.Utils
{
    public class TestUtilities
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            return config.CreateMapper();
        }

        public static ICacheService CreateCacheService()
        {
            IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);
            return cacheService;
        }
        public static Repository<T> CreateRepository<T>() where T : BaseEntity
        {
            var context = new OnlineLibriaryDBContext(new DbContextOptionsBuilder<OnlineLibriaryDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options, ensureDeleted: true);
            SeedData(context);
            return new Repository<T>(context);
        }
        public static IUnitOfWork CreateUnitOfWork()
        {
            var context = new OnlineLibriaryDBContext(new DbContextOptionsBuilder<OnlineLibriaryDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options, ensureDeleted: true);
            SeedData(context);
            return new UnitOfWork(context);
        }

        public static ISecurePasswordHasher CreateSecurePasswordHasher()
        {
            return new SecurePasswordHasher();
        }

        public static void SeedData(OnlineLibriaryDBContext context)
        {
            var genres = CreateGenres();
            var authors = CreateAuthors();
            var books = CreateBooks();
            var users = CreateUsers();
            var securePasswordHasher = new SecurePasswordHasher();

            for (int i = 0; i < 10; i++)
            {
                users[i].Books.Add(books[i]);
                users[i].Password = securePasswordHasher.Hash(users[i].Password);
            }

            context.Genres.AddRange(genres);
            context.Authors.AddRange(authors);
            context.Books.AddRange(books);
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        public static List<Genre> CreateGenres() 
        {
            var result = new List<Genre>();
            for (int i = 1; i <= 10; i++)
            {
                result.Add(new Genre
                {
                    Id = i,
                    Name = $"Genre{i}"
                });
            }
            return result;
        }
        public static List<Author> CreateAuthors()
        {
            var result = new List<Author>();
            for (int i = 1; i <= 10; i++)
            {
                result.Add(new Author
                {
                    Id = i,
                    FirstName = $"FirstName{i}",
                    SurName = $"SurName{i}",
                    Country = $"Country{i}",
                    DateOfBirth = DateTime.Now
                });
            }
            return result;
        }
        public static List<Book> CreateBooks()
        {
            var result = new List<Book>();
            for (int i = 1; i <= 10; i++)
            {
                result.Add(new Book
                {
                    Id = i,
                    Title = $"Title{i}",
                    Description = $"Description{i}",
                    AuthorId = i,
                    GenreId = i,
                    Year = DateTime.Now.Year,
                    BookContent = Array.Empty<byte>()
                });
            }
            return result;
        }
        public static List<User> CreateUsers()
        {
            var result = new List<User>();
            for (int i = 1; i <= 10; i++)
            {
                result.Add(new User
                {
                    Id = i,
                    Username = $"Username{i}",
                    FirstName = $"FirstName{i}",
                    SurName = $"SurName{i}",
                    Email = $"email{i}@gmail.com",
                    Password = $"Password{i}",
                    Books = new List<Book>(),
                    ProfilePicture = Array.Empty<byte>()
                });
            }
            return result;
        }
    }
}
