using DNTProfiler.PluginsBase;
using DNTProfiler.UnparametrizedWhereClauses.ViewModels;

namespace DNTProfiler.UnparametrizedWhereClauses
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