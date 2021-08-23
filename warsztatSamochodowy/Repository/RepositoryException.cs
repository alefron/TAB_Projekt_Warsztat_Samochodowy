using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace warsztatSamochodowy.Repository
{
    public class RepositoryException:Exception
    {

        public RepositoryException()
        {

        }
        public RepositoryException(string? message):base(message)
        {

        }
        public RepositoryException(string? message, Exception? innerException):base(message,innerException)
        {

        }
    }
}
