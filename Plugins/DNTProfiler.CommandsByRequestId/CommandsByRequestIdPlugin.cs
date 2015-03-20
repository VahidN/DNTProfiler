using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByRequestId
{
    public class CommandsByRequestIdPlugin : ProfilerPluginBase
    {
        public CommandsByRequestIdPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By Request IDs",
                PluginDescription = "Visualizing Requests Activities."
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