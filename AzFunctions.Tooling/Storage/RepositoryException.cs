using System;
using System.Collections.Generic;
using System.Text;

namespace AzFunctions.Tooling.Storage
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
    }
}
