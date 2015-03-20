using DNTProfiler.CommandsByJoinsCount.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByJoinsCount
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