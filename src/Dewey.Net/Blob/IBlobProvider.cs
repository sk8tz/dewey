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

        string GetUrl(string container, string name);
        Task<string> GetUrlAsync(string container, string name);

        Uri GetUri(string container, string name);
        Task<Uri> GetUriAsync(string container, string name);

        bool Exists(string container, string name);
        Task<bool> ExistsAsync(string container, string name);

        void CreateContainer(string container);
        Task CreateContainerAsync(string container);
    }
}
