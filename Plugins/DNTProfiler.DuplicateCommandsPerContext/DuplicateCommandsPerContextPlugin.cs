using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateCommandsPerContext
{
    public class DuplicateCommandsPerContextPlugin : ProfilerPluginBase
    {
        public DuplicateCommandsPerContextPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Duplicate Commands Per Context",
                PluginDescription = "Duplicate commands per context are the sign of excessive lazy loading or possible multiple enumeration of IEnumerable. Try using `Include` or `ToList` methods to fix it."
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