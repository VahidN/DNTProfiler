using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByExecutionTime
{
    public class CommandsByExecutionTimePlugin: ProfilerPluginBase
    {
        public CommandsByExecutionTimePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By Execution Time",
                PluginDescription = "Visualizing the most time consuming commands."
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