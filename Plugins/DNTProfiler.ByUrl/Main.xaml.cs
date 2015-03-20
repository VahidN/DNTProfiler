using DNTProfiler.ByUrl.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByUrl
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