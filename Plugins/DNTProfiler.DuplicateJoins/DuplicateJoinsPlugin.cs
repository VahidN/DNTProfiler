using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateJoins
{
    public class DuplicateJoinsPlugin : ProfilerPluginBase
    {
        public DuplicateJoinsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Duplicate Joins",
                PluginDescription = "Having duplicate joins to the same table, in the query."
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