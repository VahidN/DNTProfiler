using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.PossibleSqlInjections
{
    public class PossibleSqlInjectionsPlugin : ProfilerPluginBase
    {
        public PossibleSqlInjectionsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Possible SQL Injections",
                PluginDescription = "Unparameterized queries using the `Database.SqlQuery/ExecuteSqlCommand` methods directly, could be target of possible SQL injection attacks."
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