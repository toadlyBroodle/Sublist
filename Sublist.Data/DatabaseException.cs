using System;

namespace Sublist.Data
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message)
            : base(message)
        {
            
        }
    }
}