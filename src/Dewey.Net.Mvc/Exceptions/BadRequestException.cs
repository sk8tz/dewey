using System;

namespace Dewey.Net.Mvc.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(Exception ex)
            : base(ex.Message, ex.InnerException)
        {
        }
    }
}
