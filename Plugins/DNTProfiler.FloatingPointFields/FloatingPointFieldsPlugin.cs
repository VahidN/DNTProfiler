using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FloatingPointFields
{
    public class FloatingPointFieldsPlugin : ProfilerPluginBase
    {
        public FloatingPointFieldsPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Floating Point Fields",
                PluginDescription = "Floating point fields should not be used for entities."+
                " They can't be compared for equality. "+
                "You could lose precision with your numbers during type conversions."+
                " If it's a price field, you will need additional information to specify its type."+
                " Try using Decimal instead of floating point numbers in your entities."
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