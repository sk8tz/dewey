using System;

namespace Dewey.Net.Mvc.Exceptions
{
    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message)
            : base(message)
        {
        }

        public ForbiddenException(Exception ex)
            : base(ex.Message, ex.InnerException)
        {
        }
    }
}
