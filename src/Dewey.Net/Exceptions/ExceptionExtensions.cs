using System;
using System.Diagnostics;

namespace Dewey.Net.Exceptions
{
    public static class ExceptionExtensions
    {
        public static void Throw(this Exception ex)
        {
            if (ex == null) {
                throw new ArgumentNullException(nameof(ex));
            }

            Debug.WriteLine(ex.Message);
            Console.WriteLine(ex.Message);

            if (ex.InnerException != null) {
                Debug.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.InnerException.Message);
            }

            throw ex;
        }
    }
}
