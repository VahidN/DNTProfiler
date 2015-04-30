using DNTProfiler.FloatingPointFields.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FloatingPointFields
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