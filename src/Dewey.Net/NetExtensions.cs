using System.Net;
using System.Net.Sockets;

namespace Dewey.Net
{
    public static class NetExtensions
    {
        public static string GetIpAddress()
        {
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName())) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
    }
}
