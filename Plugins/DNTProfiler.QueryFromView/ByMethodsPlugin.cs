using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.QueryFromView
{
    public class ByMethodsPlugin : ProfilerPluginBase
    {
        public ByMethodsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Query From View",
                PluginDescription = "Doing queries from ASP.NET views is the sign of a bad design and also excessive lazy loading."
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