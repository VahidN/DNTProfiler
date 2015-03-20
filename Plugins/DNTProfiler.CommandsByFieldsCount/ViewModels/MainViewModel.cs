using System.Collections.Specialized;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByFieldsCount.ViewModels
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

            GuiModelData.PlotModel.Title = "Commands By Fields Count";
            AddXAxis("Command (Id)");
            AddYAxis("Fields Count");
            InitPlotModelEvents();
            InitUpdatePlotInterval();
        }

        private void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandResult item in e.NewItems)
                    {
                        if (item.CommandId == null || item.FieldsCount == null)
                            continue;

                        AddDataPoint(item.ApplicationIdentity,
                            item.CommandId.Value, item.FieldsCount.Value);

                        PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                        CallbacksManagerBase.UpdateAppIdentityNotificationsCount(item);
                    }
                    break;
            }
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Results.CollectionChanged += Results_CollectionChanged;
        }
    }
}