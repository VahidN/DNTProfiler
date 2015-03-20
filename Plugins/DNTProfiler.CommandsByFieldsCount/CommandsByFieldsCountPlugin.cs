using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByFieldsCount
{
    public class CommandsByFieldsCountPlugin : ProfilerPluginBase
    {
        public CommandsByFieldsCountPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By Fields Count",
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