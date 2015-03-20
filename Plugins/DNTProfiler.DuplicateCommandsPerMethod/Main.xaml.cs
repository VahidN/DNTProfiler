using DNTProfiler.DuplicateCommandsPerMethod.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateCommandsPerMethod
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