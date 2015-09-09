using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Dewey.Net.Blob.Providers
{
    public class LocalBlobProvider : IBlobProvider
    {
        public static string BlobDirectory { get; private set; }

        public LocalBlobProvider()
        {
            BlobDirectory = Path.GetTempPath();
        }

        public LocalBlobProvider(string blobDirectory)
        {
            BlobDirectory = blobDirectory;
        }

        public byte[] Download(string container, string name)
        {
            var uri = GetUri(container, name);

            using (var client = new WebClient()) {
                var result = client.DownloadData(uri);

                return result;
            }
        }

        public async Task<byte[]> DownloadAsync(string container, string name)
        {
            var uri = GetUri(container, name);

            using (var client = new WebClient()) {
                var result = await client.DownloadDataTaskAsync(uri);

                return result;
            }
        }

        public string GetUrl(string container, string name)
        {
            return GetUri(container, name).AbsolutePath;
        }

        public async Task<string> GetUrlAsync(string container, string name)
        {
            var result = await GetUriAsync(container, name);

            return result.AbsolutePath;
        }

        public Uri GetUri(string container, string name)
        {
            var path = Path.Combine(BlobDirectory, name);

            return new Uri(path);
        }

        public Task<Uri> GetUriAsync(string container, string name)
        {
            var result = GetUri(container, name);

            return Task.FromResult(result);
        }

        public void Upload(string container, string name, byte[] data, bool overwrite = true)
        {
            if (!overwrite) {
                var exists = Exists(container, name);

                if (exists) {
                    return;
                }
            }

            var uri = GetUri(container, name);

            using (var client = new WebClient()) {
                client.UploadData(uri, data);
            }
        }

        public async Task UploadAsync(string container, string name, byte[] data, bool overwrite = true)
        {
            if (!overwrite) {
                var exists = Exists(container, name);

                if (exists) {
                    return;
                }
            }

            var uri = GetUri(container, name);

            using (var client = new WebClient()) {
                await client.UploadDataTaskAsync(uri, data);
            }
        }

        public void Upload(string container, string name, Stream stream, bool overwrite = true)
        {
            var data = GetBytes(stream);

            Upload(container, name, data, overwrite);
        }

        public async Task UploadAsync(string container, string name, Stream stream, bool overwrite = true)
        {
            var data = GetBytes(stream);

            await UploadAsync(container, name, data, overwrite);
        }

        public bool Exists(string container, string name)
        {
            var url = GetUrl(container, name);

            return File.Exists(url);
        }

        public Task<bool> ExistsAsync(string container, string name)
        {
            var url = GetUrl(container, name);

            var result = File.Exists(url);

            return Task.FromResult(result);
        }

        public void CreateContainer(string container)
        {
            var path = Path.Combine(BlobDirectory, container);

            if (!Directory.Exists(path)) {
                File.Create(path);
            }
        }

        public Task CreateContainerAsync(string container)
        {
            var path = Path.Combine(BlobDirectory, container);

            if (!Directory.Exists(path)) {
                File.Create(path);
            }

            return Task.FromResult(true);
        }

        private byte[] GetBytes(Stream stream)
        {
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}
