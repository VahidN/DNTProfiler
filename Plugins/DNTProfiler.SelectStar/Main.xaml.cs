using DNTProfiler.PluginsBase;
using DNTProfiler.SelectStar.ViewModels;

namespace DNTProfiler.SelectStar
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