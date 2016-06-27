using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

namespace Dewey.Json
{
    public class JsonUtils
    {
        private static Encoding encoding;

        public static string BaseAddress { get; set; }
        
        public static async Task<JsonResult<T>> Get<T>(string url)
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url).ConfigureAwait(false);

                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return new JsonResult<T>
                    {
                        StatusCode = response.StatusCode,
                        Result = JsonConvert.DeserializeObject<T>(result)
                    };
                }

                return new JsonResult<T>
                {
                    StatusCode = response.StatusCode,
                };
            }
        }

        public static async Task Delete(string url)
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode) {
                    return;
                }

                var result = await response.Content.ReadAsStringAsync();

                throw new Exception($"Failed with status code {response.StatusCode.ToString()}", new Exception(result));
            }
        }

        public static async Task<JsonResult<T>> Delete<T>(string url)
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadAsStringAsync();

                    return new JsonResult<T>
                    {
                        StatusCode = response.StatusCode,
                        Result = JsonConvert.DeserializeObject<T>(result)
                    };
                }

                return new JsonResult<T>
                {
                    StatusCode = response.StatusCode,
                };
            }
        }

        public static async Task<JsonResult<T>> Post<T>(string url, T data) => await Post<T, T>(url, data);

        public static async Task<JsonResult<U>> Post<T, U>(string url, T data)
        {
            var serialized = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(url, new StringContent(serialized, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadAsStringAsync();

                    return new JsonResult<U> {
                        StatusCode = response.StatusCode,
                        Result = JsonConvert.DeserializeObject<U>(result)
                    };
                }

                return new JsonResult<U> {
                    StatusCode = response.StatusCode,
                };
            }
        }

        public static async Task<JsonResult<U>> Post<T, U>(string url, T data, CancellationToken cancellationToken)
        {
            var serialized = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(url, new StringContent(serialized, Encoding.UTF8, "application/json"), cancellationToken);

                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadAsStringAsync();

                    return new JsonResult<U>
                    {
                        StatusCode = response.StatusCode,
                        Result = JsonConvert.DeserializeObject<U>(result)
                    };
                }

                return new JsonResult<U>
                {
                    StatusCode = response.StatusCode,
                };
            }
        }
    }
}
