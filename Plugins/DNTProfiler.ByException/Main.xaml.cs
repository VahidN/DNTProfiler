using DNTProfiler.ByException.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByException
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