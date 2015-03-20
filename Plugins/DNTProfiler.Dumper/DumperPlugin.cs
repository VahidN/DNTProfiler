using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.Dumper
{
    public class DumperPlugin : ProfilerPluginBase
    {
        public DumperPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Save And Replay",
                PluginDescription = "Dumper Plugin writes all of the received JSON contents from the Web API to a file."
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