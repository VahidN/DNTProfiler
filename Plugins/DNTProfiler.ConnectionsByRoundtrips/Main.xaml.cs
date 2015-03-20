using DNTProfiler.ConnectionsByRoundtrips.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ConnectionsByRoundtrips
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