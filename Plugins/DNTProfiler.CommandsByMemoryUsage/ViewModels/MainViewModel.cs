using System;
using System.Collections.Specialized;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByMemoryUsage.ViewModels
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

            GuiModelData.PlotModel.Title = "Commands By Memory Usage";
            AddXAxis("Command (Id)");
            AddYAxis("Allocated Memory Size (KB)");
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
                        if (item.CommandId == null)
                            continue;

                        var command = PluginContext.ProfilerData.Commands
                            .FirstOrDefault(x => x.CommandId == item.CommandId &&
                                                 x.ApplicationIdentity.Equals(item.ApplicationIdentity));

                        if (command == null)
                            continue;

                        if (command.CommandMemoryUsage == null)
                        {
                            command.CommandMemoryUsage =
                                item.AppDomainSnapshot.TotalAllocatedMemorySize -
                                command.AppDomainSnapshot.TotalAllocatedMemorySize;
                        }

                        AddDataPoint(item.ApplicationIdentity, item.CommandId.Value,
                           Math.Round(command.CommandMemoryUsage.Value / Math.Pow(1024, 1), 1));

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