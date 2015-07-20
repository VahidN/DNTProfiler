using DNTProfiler.LongConnections.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.LongConnections
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