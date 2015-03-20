using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.InCorrectNullComparisons
{
    public class InCorrectNullComparisonsPlugin : ProfilerPluginBase
    {
        public InCorrectNullComparisonsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Incorrect Null Comparisons",
                PluginDescription = "Incorrect Null Comparisons: `select * from myTable where field1 = null`. It should be `select * from myTable where field1 IS null`."
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