using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ArithmeticOverflow
{
    public class ArithmeticOverflowPlugin : ProfilerPluginBase
    {
        public ArithmeticOverflowPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Arithmetic Overflow",
                PluginDescription = "Queries like `select sum(f1) from tbl1;` are suspected to `Arithmetic Overflow` errors."
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