using DNTProfiler.PluginsBase;
using DNTProfiler.QueryFromView.ViewModels;

namespace DNTProfiler.QueryFromView
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