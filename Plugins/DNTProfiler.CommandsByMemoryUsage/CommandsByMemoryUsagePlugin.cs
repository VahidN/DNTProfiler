using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByMemoryUsage
{
    public class CommandsByMemoryUsagePlugin : ProfilerPluginBase
    {
        public CommandsByMemoryUsagePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By Memory Usage",
                PluginDescription = "Visualizing the most expensive commands."
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