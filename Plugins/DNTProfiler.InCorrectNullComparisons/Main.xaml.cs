using DNTProfiler.InCorrectNullComparisons.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.InCorrectNullComparisons
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