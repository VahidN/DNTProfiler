using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByCommand
{
    public class ByCommandPlugin : ProfilerPluginBase
    {
        public ByCommandPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Commands",
                PluginDescription = "By Commands Plugin, represents the issued SQL commands by EF."
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