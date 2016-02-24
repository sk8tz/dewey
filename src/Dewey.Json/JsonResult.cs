using System.Net;

namespace Dewey.Json
{
    public class JsonResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public T Result { get; set; }
    }
}
