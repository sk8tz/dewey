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
            var uri = GetBlobUri(container, name);

            using (var client = new WebClient()) {
                var result = client.DownloadData(uri);

                return result;
            }
        }

        public async Task<byte[]> DownloadAsync(string container, string name)
        {
            var uri = GetBlobUri(container, name);

            using (var client = new WebClient()) {
                var result = await client.DownloadDataTaskAsync(uri);

                return result;
            }
        }

        public string GetContainerUrl(string container)
        {
            return GetContainerUri(container).AbsolutePath;
        }

        public async Task<string> GetContainerUrlAsync(string container)
        {
            var result = await GetContainerUriAsync(container);

            return result.AbsolutePath;
        }

        public string GetBlobUrl(string container, string name)
        {
            return GetBlobUri(container, name).AbsolutePath;
        }

        public async Task<string> GetBlobUrlAsync(string container, string name)
        {
            var result = await GetBlobUriAsync(container, name);

            return result.AbsolutePath;
        }

        public Uri GetContainerUri(string container)
        {
            var path = Path.Combine(BlobDirectory, container);

            return new Uri(path);
        }

        public Task<Uri> GetContainerUriAsync(string container)
        {
            var result = GetContainerUri(container);

            return Task.FromResult(result);
        }

        public Uri GetBlobUri(string container, string name)
        {
            var path = Path.Combine(BlobDirectory, container);

            path = Path.Combine(path, name);

            return new Uri(path);
        }

        public Task<Uri> GetBlobUriAsync(string container, string name)
        {
            var result = GetBlobUri(container, name);

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

            var uri = GetBlobUri(container, name);

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

            var uri = GetBlobUri(container, name);

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
            var url = GetBlobUrl(container, name);

            return File.Exists(url);
        }

        public async Task<bool> ExistsAsync(string container, string name)
        {
            var url = await GetBlobUrlAsync(container, name);

            return File.Exists(url);
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
