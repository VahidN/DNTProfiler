using DNTProfiler.CommandsByCpuUsage.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByCpuUsage
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