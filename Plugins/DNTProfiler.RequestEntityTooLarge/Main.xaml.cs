using DNTProfiler.PluginsBase;
using DNTProfiler.RequestEntityTooLarge.ViewModels;

namespace DNTProfiler.RequestEntityTooLarge
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