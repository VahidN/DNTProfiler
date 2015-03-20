using DNTProfiler.ByMethods.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByMethods
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