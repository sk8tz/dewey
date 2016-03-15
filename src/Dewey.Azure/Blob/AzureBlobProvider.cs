using Dewey.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dewey.Azure.Blob
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
                await UploadAsync(container, name, stream, overwrite);
            }
        }

        public void Upload(string container, string name, Stream stream, bool overwrite = true)
        {
            var blob = GetBlob(container, name);

            if (!overwrite && blob.Exists()) {
                return;
            }

            CreateContainer(container);

            // Create or overwrite the blob with contents from local file.
            blob.UploadFromStream(stream);
        }

        public async Task UploadAsync(string container, string name, Stream stream, bool overwrite = true)
        {
            var blob = GetBlob(container, name);

            if (!overwrite && blob.Exists()) {
                return;
            }

            await CreateContainerAsync(container);

            // Create or overwrite the blob with contents from local file.
            await blob.UploadFromStreamAsync(stream);
        }

        public string GetBlobUrl(string container, string name) => GetBlobUri(container, name).AbsolutePath;

        public async Task<string> GetBlobUrlAsync(string container, string name)
        {
            var result = await GetBlobUriAsync(container, name);

            return result.AbsolutePath;
        }

        public Uri GetBlobUri(string container, string name) => GetBlob(container, name).StorageUri.PrimaryUri;

        public Task<Uri> GetBlobUriAsync(string container, string name) => Task.FromResult(GetBlob(container, name).StorageUri.PrimaryUri);

        public string GetContainerUrl(string container) => GetContainerUri(container)?.AbsolutePath;

        public async Task<string> GetContainerUrlAsync(string container)
        {
            var result = await GetContainerUriAsync(container);

            return result?.AbsolutePath;
        }

        public Uri GetContainerUri(string container) => GetContainer(container)?.StorageUri?.PrimaryUri;

        public Task<Uri> GetContainerUriAsync(string container) => Task.FromResult(GetContainer(container)?.StorageUri?.PrimaryUri);

        public bool Exists(string container, string name) => GetBlob(container, name).Exists();

        public Task<bool> ExistsAsync(string container, string name) => Task.FromResult(GetBlob(container, name).Exists());

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

        private CloudBlobClient GetClient() => CloudStorageAccount.Parse(ConnectionString).CreateCloudBlobClient();

        private CloudBlobContainer GetContainer(string container) => GetClient().GetContainerReference(container);

        private CloudBlockBlob GetBlob(string container, string name) => GetContainer(container).GetBlockBlobReference(name);

        public void DeleteBlob(string container, string name) => GetBlob(container, name).Delete();

        public async Task DeleteBlobAsync(string container, string name) => await GetBlob(container, name).DeleteAsync();

        public void DeleteContainer(string container) => GetContainer(container).Delete();

        public async Task DeleteContainerAsync(string container) => await GetContainer(container).DeleteAsync();
    }
}
