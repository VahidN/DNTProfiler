using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.StaticResourcesQueries
{
    public class StaticResourcesQueriesPlugin : ProfilerPluginBase
    {
        public StaticResourcesQueriesPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Static Resources Queries",
                PluginDescription = "Static Resources Queries Plugin, represents the issued queries from the static resources/files requests."
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