using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.SelectStar
{
    public class SelectStarPlugin : ProfilerPluginBase
    {
        public SelectStarPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Select *",
                PluginDescription = "SELECT * makes the Table / Index Scan Monster come!"
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