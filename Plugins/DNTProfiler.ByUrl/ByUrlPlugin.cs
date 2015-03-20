using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByUrl
{
    public class ByUrlPlugin : ProfilerPluginBase
    {
        public ByUrlPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Urls",
                PluginDescription = "By Urls Plugin, represents the sorted issued SQL commands by Url."
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