using DNTProfiler.ArithmeticOverflow.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ArithmeticOverflow
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