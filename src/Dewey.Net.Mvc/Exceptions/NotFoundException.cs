using System;

namespace Dewey.Net.Mvc.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(Exception ex)
            : base(ex.Message, ex.InnerException)
        {
        }
    }
}
