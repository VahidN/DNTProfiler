using DNTProfiler.PluginsBase;
using DNTProfiler.UnboundedResultSets.ViewModels;

namespace DNTProfiler.UnboundedResultSets
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