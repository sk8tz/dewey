﻿using System;
using System.IO;
using System.Threading.Tasks;
using Dewey.Net.Blob.Providers;

namespace Dewey.Net.Blob
{
    public class BlobManager : IDisposable
    {
        public IBlobProvider Provider { get; set; }

        public BlobManager()
        {
            if (BlobSettings.DefaultProvider == null) {
                Provider = new LocalBlobProvider();
            } else {
                Provider = BlobSettings.DefaultProvider;
            }
        }

        public BlobManager(IBlobProvider provider)
        {
            Provider = provider;
        }

        public byte[] Download(string container, string name)
        {
            return Provider.Download(container, name);
        }

        public async Task<byte[]> DownloadAsync(string container, string name)
        {
            return await Provider.DownloadAsync(container, name);
        }

        public string GetUrl(string container, string name)
        {
            return Provider.GetUrl(container, name);
        }

        public async Task<string> GetUrlAsync(string container, string name)
        {
            return await Provider.GetUrlAsync(container, name);
        }

        public void Upload(string container, string name, byte[] data)
        {
            Provider.Upload(container, name, data);
        }

        public async Task UploadAsync(string container, string name, byte[] data)
        {
            await Provider.UploadAsync(container, name, data);
        }

        public void Upload(string container, string name, Stream stream)
        {
            Provider.Upload(container, name, stream);
        }

        public async Task UploadAsync(string container, string name, Stream stream)
        {
            await Provider.UploadAsync(container, name, stream);
        }

        public bool Exists(string container, string name)
        {
            return Provider.Exists(container, name);
        }

        public async Task<bool> ExistsAsync(string container, string name)
        {
            return await Provider.ExistsAsync(container, name);
        }

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
