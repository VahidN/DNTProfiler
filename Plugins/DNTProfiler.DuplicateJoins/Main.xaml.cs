using DNTProfiler.DuplicateJoins.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateJoins
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