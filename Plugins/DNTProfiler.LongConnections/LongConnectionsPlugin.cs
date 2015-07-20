using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.LongConnections
{
    public class LongConnectionsPlugin : ProfilerPluginBase
    {
        public LongConnectionsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Long Connections",
                PluginDescription = "EF connections won’t be closed until the ObjectResult has been completely consumed or disposed. So if you are keeping the connection open, because the UI data binding is not done yet, there will be a lot of ASYNC_NETWORK_IO waits for the query at database side, which are hazardous."
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Alerts;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}