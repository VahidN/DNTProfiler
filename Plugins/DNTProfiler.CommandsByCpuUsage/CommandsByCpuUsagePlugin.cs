using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByCpuUsage
{
    public class CommandsByCpuUsagePlugin : ProfilerPluginBase
    {
        public CommandsByCpuUsagePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By CPU Usage",
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