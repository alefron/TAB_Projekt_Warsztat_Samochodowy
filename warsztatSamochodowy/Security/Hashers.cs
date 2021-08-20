using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Security
{
    public static class Hashers
    {
        public static readonly IHasher Hasher;
        static Hashers()
        {
            Hasher = new MockHasher();
        }

    }
}
