using System.Collections.Specialized;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByRowsReturned.ViewModels
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

            GuiModelData.PlotModel.Title = "Commands By Rows Returned";
            AddXAxis("Command (Id)");
            AddYAxis("Rows Returned");
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
                        if (item.CommandId != null && item.RowsReturned != null)
                        {
                            AddDataPoint(item.ApplicationIdentity, item.CommandId.Value,
                                item.RowsReturned.Value);

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