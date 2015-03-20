using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByJoinsCount
{
    public class CommandsByJoinsCountPlugin : ProfilerPluginBase
    {
        public CommandsByJoinsCountPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By Joins Count",
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