using DNTProfiler.CommandsByRowsReturned.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByRowsReturned
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