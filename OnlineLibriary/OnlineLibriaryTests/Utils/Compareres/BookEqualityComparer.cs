using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriaryTests.Utils.Compareres
{
    public class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals(Book x, Book y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.Title == y.Title &&
                   x.Description == y.Description &&
                   x.Year == y.Year &&
                   BookContentEqual(x.BookContent, y.BookContent) &&
                   x.AuthorId == y.AuthorId &&
                   x.GenreId == y.GenreId;
        }

        public int GetHashCode(Book obj)
        {
            return obj.Id.GetHashCode();
        }

        private bool BookContentEqual(byte[] content1, byte[] content2)
        {
            if (content1 == null && content2 == null)
                return true;
            if (content1 == null || content2 == null)
                return false;

            return content1.SequenceEqual(content2);
        }
    }
}
