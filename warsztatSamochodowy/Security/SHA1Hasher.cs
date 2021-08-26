using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace warsztatSamochodowy.Security
{
    public class SHA1Hasher : IHasher
    {
        public string GetHash(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder(hash.Length * 2);


                foreach(byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();

            }
        }

        public string GetSaltyHash(string password, string salt)
        {
            return GetHash(password + salt);
        }
    }
}
