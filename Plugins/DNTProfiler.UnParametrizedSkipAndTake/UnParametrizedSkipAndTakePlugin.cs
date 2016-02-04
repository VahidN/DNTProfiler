using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.UnParametrizedSkipAndTake
{
    public class UnParametrizedSkipAndTakePlugin : ProfilerPluginBase
    {
        public UnParametrizedSkipAndTakePlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Unparameterized Skip & Take",
                PluginDescription =
                "Normal Skip and Take methods in EF will produce unparameterized offset clauses. Try using Take(()=>var1) and Skip(()=>var2) overloads instead."
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