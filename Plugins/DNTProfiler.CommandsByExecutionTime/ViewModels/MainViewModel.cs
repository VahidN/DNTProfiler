using System.Collections.Specialized;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByExecutionTime.ViewModels
{
    public class MainViewModel : ChartsViewModelBase
    {
        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            SetActions();
            setEvenets();

            GuiModelData.PlotModel.Title = "Commands By Execution Time";
            AddXAxis("Command (Id)");
            AddYAxis("Execution Time (ms)");
            InitPlotModelEvents();
            InitUpdatePlotInterval();
        }

        private void CommandResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandResult item in e.NewItems)
                    {
                        if (item.CommandId != null && item.ElapsedMilliseconds != null)
                        {
                            AddDataPoint(item.ApplicationIdentity, item.CommandId.Value,
                                item.ElapsedMilliseconds.Value);

                            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                            CallbacksManagerBase.UpdateAppIdentityNotificationsCount(item);
                        }
                    }
                    break;
            }
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Results.CollectionChanged += CommandResults_CollectionChanged;
        }
    }
}