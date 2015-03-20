using DNTProfiler.CommandsByMemoryUsage.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByMemoryUsage
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