using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Security
{
    internal class MockHasher : IHasher
    {
        public string GetHash(string password)
        {
            return password;
        }

        public string GetSaltyHash(string password, string salt)
        {
            return password;
        }
    }
}
