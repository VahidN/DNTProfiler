using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.MultipleContextPerRequest
{
    public class MultipleContextPerRequestPlugin : ProfilerPluginBase
    {
        public MultipleContextPerRequestPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Multiple Contexts Per Request",
                PluginDescription = "Each new context, means creating a new connection without resuing the existing one."
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