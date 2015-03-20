using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByRowsReturned
{
    public class CommandsByRowsReturned: ProfilerPluginBase
    {
        public CommandsByRowsReturned()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Commands By Rows Returned",
                PluginDescription = "Visualizing Excessive Number Of Rows Returned."
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