using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Security
{
    public static class SecurityUtils
    {
        public static readonly IHasher Hasher;
        static SecurityUtils()
        {
            Hasher = new MockHasher();
        }

    }
}
