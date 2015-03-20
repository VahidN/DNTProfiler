using DNTProfiler.EFTrafficLogger.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.EFTrafficLogger
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