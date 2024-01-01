using Data.Entities;
using Data.Repositories;

namespace Data.Interfaces
{
    internal interface IUnitOfWork
    {        
        Repository<Author> AuthorRepository { get; }
        Repository<Book> BookRepository { get; }
        Repository<Genre> GenreRepository { get; }
        Repository<User> UserRepository { get; }
        public Task SaveAsync();
    }
}
