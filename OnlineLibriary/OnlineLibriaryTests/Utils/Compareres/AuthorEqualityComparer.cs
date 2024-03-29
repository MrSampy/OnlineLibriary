﻿using BusinessLogic.Models.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriaryTests.Utils.Compareres
{
    public class AuthorEqualityComparer : IEqualityComparer<Author>
    {
        public bool Equals(Author x, Author y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.FirstName == y.FirstName &&
                   x.SurName == y.SurName &&
                   x.DateOfBirth.ToString("dd.MM.yyyy") == y.DateOfBirth.ToString("dd.MM.yyyy") &&
                   x.Country == y.Country;
        }
        public bool Equals(AuthorDTO x, Author y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && 
                   x.FullName == $"{y.FirstName} {y.SurName}" &&
                   x.DateOfBirth.ToString("dd.MM.yyyy") == y.DateOfBirth.ToString("dd.MM.yyyy") &&
                   x.Country == y.Country;
        }
        public int GetHashCode(Author obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
