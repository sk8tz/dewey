using System;
using System.IO;
using System.Threading.Tasks;

namespace Dewey.Blob
{
    public class BlobManager : IDisposable
    {
        public IBlobProvider Provider { get; set; }

        public BlobManager()
        {
            if (BlobSettings.DefaultProvider == null) {
#if !DNXCORE50
                Provider = new Providers.LocalBlobProvider();
#endif
            } else {
                Provider = BlobSettings.DefaultProvider;
            }
        }

        public BlobManager(IBlobProvider provider)
        {
            Provider = provider;
        }

        public byte[] Download(string container, string name) => Provider.Download(container, name);

        public async Task<byte[]> DownloadAsync(string container, string name) => await Provider.DownloadAsync(container, name);

        public string GetBlobUrl(string container, string name) => Provider.GetBlobUrl(container, name);

        public async Task<string> GetBlobUrlAsync(string container, string name) => await Provider.GetBlobUrlAsync(container, name);

        public Uri GetBlobUri(string container, string name) => Provider.GetBlobUri(container, name);

        public async Task<Uri> GetBlobUriAsync(string container, string name) => await Provider.GetBlobUriAsync(container, name);

        public string GetContainerUrl(string container) => Provider.GetContainerUrl(container);

        public async Task<string> GetContainerUrlAsync(string container) => await Provider.GetContainerUrlAsync(container);

        public Uri GetContainerUri(string container) => Provider.GetContainerUri(container);

        public async Task<Uri> GetContainerUriAsync(string container) => await Provider.GetContainerUriAsync(container);

        public void Upload(string container, string name, byte[] data) => Provider.Upload(container, name, data);

        public async Task UploadAsync(string container, string name, byte[] data) => await Provider.UploadAsync(container, name, data);

        public void Upload(string container, string name, Stream stream) => Provider.Upload(container, name, stream);

        public async Task UploadAsync(string container, string name, Stream stream) => await Provider.UploadAsync(container, name, stream);

        public bool Exists(string container, string name) => Provider.Exists(container, name);

        public async Task<bool> ExistsAsync(string container, string name) => await Provider.ExistsAsync(container, name);

        public void DeleteBlob(string container, string name) => Provider.DeleteBlob(container, name);

        public async Task DeleteBlobAsync(string container, string name) => await Provider.DeleteBlobAsync(container, name);

        public void DeleteContainer(string container) => Provider.DeleteContainer(container);

        public async Task DeleteContainerAsync(string container) => await Provider.DeleteContainerAsync(container);

        #region IDisposable Support
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
