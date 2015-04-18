using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DNTProfiler.Services
{
    public class SelfHostConfig
    {
        private HttpSelfHostServer _server;
        public void OpenWait(string serverUri, bool allowRemoteConnections)
        {
            var config = getHostConfiguration(serverUri, allowRemoteConnections);
            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
        }

        public void Stop()
        {
            if (_server == null)
                return;

            _server.CloseAsync().Wait();
            _server.Dispose();
            _server = null;
        }

        private static HttpSelfHostConfiguration getHostConfiguration(string serverUri, bool allowRemoteConnections)
        {
            var config = new HttpSelfHostConfiguration(serverUri)
            {
                // to avoid `(413) Request Entity Too Large` exceptions
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue
            };

            if (!allowRemoteConnections)
            {
                // Only clients from the local machine will be able to invoke the hosted REST API.
                // It does NOT require admin privileges or a URL reservation.
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;
            }

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
            return config;
        }
    }
}