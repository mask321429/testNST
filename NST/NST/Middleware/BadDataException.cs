using System.Runtime.Serialization;

namespace NST.Middleware
{
    public class BadDataException : Exception
    {
        public BadDataException(string message) : base(message)
        {
        }
    }
}