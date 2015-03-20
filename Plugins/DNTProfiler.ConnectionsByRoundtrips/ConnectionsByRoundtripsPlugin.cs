using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ConnectionsByRoundtrips
{
    public class ConnectionsByRoundtripsPlugin : ProfilerPluginBase
    {
        public ConnectionsByRoundtripsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Connections By Roundtrips",
                PluginDescription = "Visualizing the most active connectins and possible incorrect lazy loadings."
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Visualizers;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}