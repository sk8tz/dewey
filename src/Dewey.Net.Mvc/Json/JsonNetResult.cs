using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Mvc;

namespace Dewey.Net.Mvc.Json
{
    public class JsonNetResult<T> : JsonResult
    {
        private readonly HttpStatusCode _statusCode = HttpStatusCode.OK;

        public JsonNetResult()
        {

        }

        public JsonNetResult(T data)
        {
            Data = data;
        }

        public JsonNetResult(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        public JsonNetResult(T data, HttpStatusCode statusCode)
        {
            Data = data;
            _statusCode = statusCode;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            if (_statusCode != HttpStatusCode.OK) {
                response.StatusCode = (int)_statusCode;
            }

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null) {
                response.ContentEncoding = ContentEncoding;
            }

            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.None);

            response.Write(serializedObject);
        }
    }
}
