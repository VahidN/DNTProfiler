using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.RequestEntityTooLarge
{
    public class RequestEntityTooLargePlugin : ProfilerPluginBase
    {
        public RequestEntityTooLargePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Large Query Entities",
                PluginDescription = "The received SQL string size is too large (> 20 KB)."
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