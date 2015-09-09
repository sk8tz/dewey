using Dewey.Net.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dewey.Net.Azure.Blob
{
    public class AzureBlobProvider : IBlobProvider
    {
        public static string ConnectionString { get; private set; }

        public AzureBlobProvider()
        {
        }

        public AzureBlobProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public byte[] Download(string container, string name)
        {
            byte[] result;

            var blob = GetBlob(container, name);

            using (var memoryStream = new MemoryStream()) {
                blob.DownloadToStream(memoryStream);
                result = memoryStream.ToArray();
            }

            return result;
        }

        public async Task<byte[]> DownloadAsync(string container, string name)
        {
            byte[] result;

            var blob = GetBlob(container, name);

            using (var memoryStream = new MemoryStream()) {
                await blob.DownloadToStreamAsync(memoryStream);
                result = memoryStream.ToArray();
            }

            return result;
        }

        public void Upload(string container, string name, byte[] data, bool overwrite = true)
        {
            using (var stream = new MemoryStream(data)) {
                Upload(container, name, stream, overwrite);
            }
        }

        public async Task UploadAsync(string container, string name, byte[] data, bool overwrite = true)
        {
            using (var stream = new MemoryStream(data)) {
                await UploadAsync(container, name, data, overwrite);
            }
        }

        public void Upload(string container, string name, Stream stream, bool overwrite = true)
        {
            var blob = GetBlob(container, name);

            if (!overwrite && blob.Exists()) {
                return;
            }

            // Create or overwrite the blob with contents from local file.
            blob.UploadFromStream(stream);
        }

        public async Task UploadAsync(string container, string name, Stream stream, bool overwrite = true)
        {
            var blob = GetBlob(container, name);

            if (!overwrite && blob.Exists()) {
                return;
            }

            // Create or overwrite the blob with contents from local file.
            await blob.UploadFromStreamAsync(stream);
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
            var blob = GetBlob(container, name);

            return blob.StorageUri.PrimaryUri;
        }

        public Task<Uri> GetUriAsync(string container, string name)
        {
            var blob = GetBlob(container, name);

            var result = blob.StorageUri.PrimaryUri;

            return Task.FromResult(result);
        }

        public bool Exists(string container, string name)
        {
            return GetBlob(container, name).Exists();
        }

        public Task<bool> ExistsAsync(string container, string name)
        {
            var result = GetBlob(container, name).Exists();

            return Task.FromResult(result);
        }

        public void CreateContainer(string container)
        {
            var client = CloudStorageAccount.Parse(ConnectionString)
                                            .CreateCloudBlobClient();

            // Retrieve a reference to a container.
            var newContainer = client.GetContainerReference(container);

            // Create the container if it doesn't already exist.
            newContainer.CreateIfNotExists();

            //make it publicly accessible
            newContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        public async Task CreateContainerAsync(string container)
        {
            var client = CloudStorageAccount.Parse(ConnectionString)
                                            .CreateCloudBlobClient();

            // Retrieve a reference to a container.
            var newContainer = client.GetContainerReference(container);

            // Create the container if it doesn't already exist.
            await newContainer.CreateIfNotExistsAsync();

            //make it publicly accessible
            await newContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        private CloudBlobClient GetClient()
        {
            return CloudStorageAccount.Parse(ConnectionString)
                                      .CreateCloudBlobClient();
        }

        private CloudBlobContainer GetContainer(string container)
        {
            return GetClient().GetContainerReference(container);
        }

        private CloudBlockBlob GetBlob(string container, string name)
        {
            return GetContainer(container).GetBlockBlobReference(name);
        }
    }
}
