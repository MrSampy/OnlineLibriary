using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ISecurePasswordHasher
    {
        public string Hash(string password, int iterations);
        public string Hash(string password);
        public bool Verify(string password, string hashedPassword);
        public bool Verify(string password, string hashedPassword, int iterations);
    }
}
