using DNTProfiler.PluginsBase;
using DNTProfiler.UnParametrizedSkipAndTake.ViewModels;

namespace DNTProfiler.UnParametrizedSkipAndTake
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