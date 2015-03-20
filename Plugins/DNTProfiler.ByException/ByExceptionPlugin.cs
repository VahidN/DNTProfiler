using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByException
{
    public class ByExceptionPlugin : ProfilerPluginBase
    {
        public ByExceptionPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Exceptions",
                PluginDescription = "By Exceptions Plugin, Collects the exceptional issued SQL commands by EF."
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Alerts;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}