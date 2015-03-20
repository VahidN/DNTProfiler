using System.Collections.Specialized;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByJoinsCount.ViewModels
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

            GuiModelData.PlotModel.Title = "Commands By Joins Count";
            AddXAxis("Command (Id)");
            AddYAxis("Joins Count");
            InitPlotModelEvents();
            InitUpdatePlotInterval();
        }

        private void Commands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Command item in e.NewItems)
                    {
                        if (item.CommandId == null)
                            continue;

                        AddDataPoint(item.ApplicationIdentity,
                            item.CommandId.Value, item.CommandStatistics.JoinsCount);
                        PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                        CallbacksManagerBase.UpdateAppIdentityNotificationsCount(item);
                    }
                    break;
            }
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += Commands_CollectionChanged;
        }
    }
}