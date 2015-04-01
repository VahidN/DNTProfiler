using DNTProfiler.PluginsBase;
using DNTProfiler.PossibleSqlInjections.ViewModels;

namespace DNTProfiler.PossibleSqlInjections
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