using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByMethods
{
    public class ByMethodsPlugin : ProfilerPluginBase
    {
        public ByMethodsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Methods",
                PluginDescription = "By Methods Plugin, represents the issued SQL commands from diffrent methods of the system."
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