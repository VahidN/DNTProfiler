using DNTProfiler.PluginsBase;
using DNTProfiler.UseFirstLevelCache.ViewModels;

namespace DNTProfiler.UseFirstLevelCache
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