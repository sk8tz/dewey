using System;
using System.IO;
using System.Threading.Tasks;

namespace Dewey.Net.Blob
{
    public interface IBlobProvider
    {
        byte[] Download(string container, string name);
        Task<byte[]> DownloadAsync(string container, string name);

        void Upload(string container, string name, byte[] data, bool overwrite = true);
        Task UploadAsync(string container, string name, byte[] data, bool overwrite = true);
        void Upload(string container, string name, Stream stream, bool overwrite = true);
        Task UploadAsync(string container, string name, Stream stream, bool overwrite = true);

        string GetContainerUrl(string container);
        Task<string> GetContainerUrlAsync(string container);

        string GetBlobUrl(string container, string name);
        Task<string> GetBlobUrlAsync(string container, string name);

        Uri GetContainerUri(string container);
        Task<Uri> GetContainerUriAsync(string container);

        Uri GetBlobUri(string container, string name);
        Task<Uri> GetBlobUriAsync(string container, string name);

        bool Exists(string container, string name);
        Task<bool> ExistsAsync(string container, string name);

        void CreateContainer(string container);
        Task CreateContainerAsync(string container);

        void DeleteBlob(string container, string name);

        Task DeleteBlobAsync(string container, string name);

        void DeleteContainer(string container);

        Task DeleteContainerAsync(string container);
    }
}
