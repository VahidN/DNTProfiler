using DNTProfiler.ByConnections.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByConnections
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