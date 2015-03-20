using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.EFTrafficLogger
{
    public class EFTrafficLoggerPlugin : ProfilerPluginBase
    {
        public EFTrafficLoggerPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Context",
                PluginDescription = "By Context Plugin, sorts all of the EF interactions with the database by context."
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Loggers;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}