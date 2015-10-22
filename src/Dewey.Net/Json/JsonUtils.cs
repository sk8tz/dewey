using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace Dewey.Net.Json
{
    public class JsonUtils
    {
        public static T Get<T>(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            try {
                var response = request.GetResponse();

                using (var responseStream = response.GetResponseStream()) {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);

                    var result = reader.ReadToEnd();

                    return JsonConvert.DeserializeObject<T>(result);
                }
            } catch (WebException ex) {
                var errorResponse = ex.Response;

                using (var responseStream = errorResponse.GetResponseStream()) {
                    var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));

                    var errorText = reader.ReadToEnd();

                    // Log Exception
                }

                throw;
            }
        }

        public static void Post(string url, string jsonContent)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";

            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (var dataStream = request.GetRequestStream()) {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            var length = 0L;

            try {
                using (var response = (HttpWebResponse)request.GetResponse()) {
                    length = response.ContentLength;
                }
            } catch (WebException ex) {
                // Log exception and throw as for GET example above
            }
        }
    }
}
