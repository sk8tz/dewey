using System.IO;

namespace Dewey.Types
{
    public static class StreamExtensions
    {
        public static byte[] GetBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public static Stream ToStream(this byte[] buffer) => new MemoryStream(buffer);
    }
}
