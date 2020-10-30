using System.Net;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace NbSites.Jobs.Hangfires
{
    public class MyDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
            //todo
            var isLocal = context.GetHttpContext().Request.IsLocal();
            if (isLocal)
            {
                return true;
            }

            var httpContext = context.GetHttpContext();
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (!httpContext.User.HasClaim("Role", "Admin"))
            {
                return false;
            }

            return true;
        }
    }

    public static class HttpRequestExtensions
    {
        public static bool IsLocal(this HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress != null)
            {
                if (connection.LocalIpAddress != null)
                {
                    return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
                }
                else
                {
                    return IPAddress.IsLoopback(connection.RemoteIpAddress);
                }
            }

            // for in memory TestServer or when dealing with default connection info
            if (connection.RemoteIpAddress == null && connection.LocalIpAddress == null)
            {
                return true;
            }

            return false;
        }
    }
}