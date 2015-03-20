using DNTProfiler.ApplicationExceptions.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationExceptions
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