using System.Web;
using System.Security.Principal;
using Dewey.Net.Types;

namespace Dewey.Net.Mvc.Http
{
    public static class HttpContextExtensions
    {
        public static bool IsLoggedIn
        {
            get
            {
                return !LoggedInUserId.IsEmpty();
            }
        }
        
        public static IIdentity LoggedInUserIdentity
        {
            get
            {
                return HttpContext.Current?.User?.Identity;
            }
        }

        public static string LoggedInUserId
        {
            get
            {
                return LoggedInUserIdentity?.Name;
            }
        }

        public static string Subdomain(this HttpContext context)
        {
            string host = HttpContext.Current.Request.Url.Host;

            if (host.ToLower().Contains("localhost") || host.ToLower().Contains("127.0.0.1")) {
                return "localhost";
            }

            if (host.Split('.').Length > 1) {
                int index = host.IndexOf(".");
                string subdomain = host.Substring(0, index);

                return subdomain;
            }

            return host;
        }

        public static string Subdomain(this HttpContextBase context)
        {
            string host = HttpContext.Current.Request.Url.Host;

            if (host.ToLower().Contains("localhost") || host.ToLower().Contains("127.0.0.1")) {
                return "localhost";
            }

            if (host.Split('.').Length > 1) {
                int index = host.IndexOf(".");
                string subdomain = host.Substring(0, index);

                return subdomain;
            }

            return host;
        }
    }
}
