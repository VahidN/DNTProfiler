using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FunctionCallsInWhereClause
{
    public class FunctionCallsInWhereClausePlugin : ProfilerPluginBase
    {
        public FunctionCallsInWhereClausePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Function Calls In Where Clause",
                PluginDescription = "When functions are used in the WHERE clause, this forces SQL Server to do a table scan or index scan to get the correct results instead of doing an index seek if there is an index that can be used."
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