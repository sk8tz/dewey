using System;

namespace Dewey.Net.Mvc.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(Exception ex)
            : base(ex.Message, ex.InnerException)
        {
        }
    }
}
