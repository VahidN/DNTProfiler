using DNTProfiler.ContextInMultipleThreads.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ContextInMultipleThreads
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