using System.Net;

namespace Dewey.Net.Json
{
    public class JsonResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Result { get; set; }
    }
}
