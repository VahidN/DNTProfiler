using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.UseFirstLevelCache
{
    public class UseFirstLevelCachePlugin : ProfilerPluginBase
    {
        public UseFirstLevelCachePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Use First Level Cache",
                PluginDescription = "By using EntityFramework's Find method, If an entity with the given primary key values exists in the context, then it is returned immediately without making a request to the store."
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