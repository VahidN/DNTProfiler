using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.UnboundedResultSets
{
    public class UnboundedResultSetsPlugin : ProfilerPluginBase
    {
        public UnboundedResultSetsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Unbounded Result Sets",
                PluginDescription = "Unbounded result set performs a query without explicitly limiting the number of returned results. "+
                                    "These queries are the main cause of out of memory errors."
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