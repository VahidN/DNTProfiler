using DNTProfiler.ApplicationAnnouncements.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationAnnouncements
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