using DNTProfiler.CommandsByRequestId.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByRequestId
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