using DNTProfiler.FunctionCallsInWhereClause.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.FunctionCallsInWhereClause
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