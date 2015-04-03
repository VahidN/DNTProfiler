using DNTProfiler.DuplicateCommandsPerContext.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateCommandsPerContext
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