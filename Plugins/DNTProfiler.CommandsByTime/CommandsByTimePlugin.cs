using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByTime
{
    public class CommandsByTimePlugin : ProfilerPluginBase
    {
        public CommandsByTimePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands Time Line",
                PluginDescription = "Visualizing Commands Time Line."
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