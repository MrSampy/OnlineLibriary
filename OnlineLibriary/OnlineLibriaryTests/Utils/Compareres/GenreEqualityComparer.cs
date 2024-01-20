using BusinessLogic.Models.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriaryTests.Utils.Compareres
{
    public class GenreEqualityComparer : IEqualityComparer<Genre>
    {
        public bool Equals(Genre x, Genre y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.Name == y.Name;
        }

        public bool Equals(GenreDTO x, Genre y) 
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.Name == y.Name;
        }

        public int GetHashCode(Genre obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
