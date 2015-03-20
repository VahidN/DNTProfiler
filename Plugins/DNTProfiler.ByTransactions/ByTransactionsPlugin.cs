using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByTransactions
{
    public class ByTransactionsPlugin : ProfilerPluginBase
    {
        public ByTransactionsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "By Transactions",
                PluginDescription = "By Transactions Plugin, represents the issued transactions by EF."
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