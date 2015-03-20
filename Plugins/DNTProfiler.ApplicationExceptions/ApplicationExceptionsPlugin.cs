using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationExceptions
{
    public class ApplicationExceptionsPlugin : ProfilerPluginBase
    {
        public ApplicationExceptionsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Exceptions",
                PluginDescription = "Shows the current application's exceptions."
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Application;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}