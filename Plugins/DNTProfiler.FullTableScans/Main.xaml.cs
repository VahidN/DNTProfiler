using DNTProfiler.FullTableScans.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FullTableScans
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