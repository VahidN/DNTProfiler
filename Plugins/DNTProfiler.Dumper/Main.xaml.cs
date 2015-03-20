using DNTProfiler.Dumper.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.Dumper
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