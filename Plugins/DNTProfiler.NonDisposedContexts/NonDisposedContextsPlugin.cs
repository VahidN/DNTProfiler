using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.NonDisposedContexts
{
    public class NonDisposedContextsPlugin : ProfilerPluginBase
    {
        public NonDisposedContextsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Non-Disposed Connections",
                PluginDescription = @"It's easy to forget not disposing object contexts," +
                " which leads to memory leaks and also leaving too many related connection objects not disposed" +
                " as well."
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