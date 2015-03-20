using DNTProfiler.ByCommand.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByCommand
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