using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.RawLogger
{
    public class RawLoggerPlugin : ProfilerPluginBase
    {
        public RawLoggerPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Raw Logger",
                PluginDescription = "RawLogger Plugin logs all of the received JSON contents from the Web API."
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