using DNTProfiler.PluginsBase;
using DNTProfiler.RawLogger.ViewModels;

namespace DNTProfiler.RawLogger
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