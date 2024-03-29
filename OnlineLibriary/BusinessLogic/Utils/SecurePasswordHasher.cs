﻿using BusinessLogic.Interfaces;
using System.Security.Cryptography;

namespace BusinessLogic.Utils
{
    public class SecurePasswordHasher : ISecurePasswordHasher
    {
        private const int SaltSize = 16;
        public const int BaseIterations = 10000;
        private const int HashSize = 20;
        public string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }
        public string Hash(string password)
        {
            return Hash(password, BaseIterations);
        }

        public bool Verify(string password, string hashedPassword, int iterations)
        {
            // Get hash bytes
            var hashBytes = Convert.FromBase64String(hashedPassword);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        //Verify with base iterations
        public bool Verify(string password, string hashedPassword)
        {
            return Verify(password, hashedPassword, BaseIterations);
        }

    }
}