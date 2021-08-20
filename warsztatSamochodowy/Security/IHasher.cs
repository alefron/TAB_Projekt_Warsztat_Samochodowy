using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Security
{
    public interface IHasher
    {
        string GetHash(string password);
        string GetSaltyHash(string password, string salt);
    }
}
