using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ContextInMultipleThreads
{
    public class ContextInMultipleThreadsPlugin : ProfilerPluginBase
    {
        public ContextInMultipleThreadsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Context In Multiple Threads",
                PluginDescription = "ObjectContex and DbContext are not thread safe. Using a single ObjectContext in multiple threads will corrupt your data."
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