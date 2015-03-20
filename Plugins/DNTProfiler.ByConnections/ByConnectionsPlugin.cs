using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByConnections
{
    public class ByConnectionsPlugin : ProfilerPluginBase
    {
        public ByConnectionsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Connections",
                PluginDescription = "By Connections Plugin, represents the issued connections by EF."
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Loggers;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}