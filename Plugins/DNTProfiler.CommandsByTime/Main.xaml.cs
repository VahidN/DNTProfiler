using DNTProfiler.CommandsByTime.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByTime
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