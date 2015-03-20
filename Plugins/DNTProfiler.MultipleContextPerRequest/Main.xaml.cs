using DNTProfiler.MultipleContextPerRequest.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.MultipleContextPerRequest
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