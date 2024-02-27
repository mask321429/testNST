using System.Runtime.Serialization;

namespace NST.Middleware
{
    public class UnAuthorizeException : Exception
    {
        public UnAuthorizeException(string message) : base(message)
        {
        }
    }
}