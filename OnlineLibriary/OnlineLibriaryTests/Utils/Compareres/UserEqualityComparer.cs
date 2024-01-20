using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using BusinessLogic.Utils;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibriaryTests.Utils.Compareres
{
    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            SecurePasswordHasher securePasswordHasher = new SecurePasswordHasher();
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.FirstName == y.FirstName &&
                   x.SurName == y.SurName &&
                   x.Email == y.Email &&
                   securePasswordHasher.Verify(x.Password, y.Password) &&
                   ProfilePicturesEqual(x.ProfilePicture, y.ProfilePicture);
        }
        public bool Equals(UserDTO x, User y)
        {
            SecurePasswordHasher securePasswordHasher = new SecurePasswordHasher();
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.FullName == $"{y.FirstName} {y.SurName}" &&
                   x.Email == y.Email &&
                   securePasswordHasher.Verify(y.Password, x.Password) &&
                   ProfilePicturesEqual(x.ProfilePicture, y.ProfilePicture);
        }
        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }

        private bool ProfilePicturesEqual(byte[] picture1, byte[] picture2)
        {
            if (picture1 == null && picture2 == null)
                return true;
            if (picture1 == null || picture2 == null)
                return false;

            return picture1.SequenceEqual(picture2);
        }
    }

}
