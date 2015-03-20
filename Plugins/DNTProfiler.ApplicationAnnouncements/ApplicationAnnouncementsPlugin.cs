using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationAnnouncements
{
    public class ApplicationAnnouncementsPlugin : ProfilerPluginBase
    {
        public ApplicationAnnouncementsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Announcements",
                PluginDescription = "Latest news of the project."
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