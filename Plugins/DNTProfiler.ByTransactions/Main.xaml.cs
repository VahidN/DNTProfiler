using DNTProfiler.ByTransactions.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByTransactions
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