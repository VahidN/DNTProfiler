using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FullTableScans
{
    public class FullTableScansPlugin : ProfilerPluginBase
    {
        public FullTableScansPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Full Table Scans",
                PluginDescription = "Full Table Scans Plugin, represents the issued SQL commands containing `LIKE '%...'` by EF."
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