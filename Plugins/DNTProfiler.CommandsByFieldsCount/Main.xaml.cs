using DNTProfiler.CommandsByFieldsCount.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByFieldsCount
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