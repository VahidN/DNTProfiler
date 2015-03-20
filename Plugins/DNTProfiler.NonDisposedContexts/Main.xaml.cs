using DNTProfiler.NonDisposedContexts.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.NonDisposedContexts
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