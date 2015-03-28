using DNTProfiler.PluginsBase;
using DNTProfiler.StaticResourcesQueries.ViewModels;

namespace DNTProfiler.StaticResourcesQueries
{
    public partial class Main
    {
        public Main(ProfilerPluginBase pluginContext)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(pluginContext);
        }
    }
}