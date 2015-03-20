using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateCommandsPerMethod
{
    public class DuplicateCommandsPerMethodPlugin : ProfilerPluginBase
    {
        public DuplicateCommandsPerMethodPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Duplicate Commands Per Method",
                PluginDescription = "Duplicate commands per method are the sign of excessive lazy loading or possible multiple enumeration of IEnumerable. Try using `Include` or `ToList` methods to fix it."
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