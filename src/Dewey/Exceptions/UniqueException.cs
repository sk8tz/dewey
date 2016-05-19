using System;

namespace Dewey.Exceptions
{
    public class UniqueException : Exception
    {
        public string Property { get; private set; }

        public UniqueException()
        {
        }

        public UniqueException(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public override string Message { get; }
    }
}
