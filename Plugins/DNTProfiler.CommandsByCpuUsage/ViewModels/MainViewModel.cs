using System.Collections.Specialized;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.CommandsByCpuUsage.ViewModels
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

            GuiModelData.PlotModel.Title = "Commands By CPU Usage";
            AddXAxis("Command (Id)");
            AddYAxis("Processor Time (ms)");
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

                        if (command.CommandCpuUsage == null)
                        {
                            command.CommandCpuUsage =
                                item.AppDomainSnapshot.TotalProcessorTime -
                                command.AppDomainSnapshot.TotalProcessorTime;
                        }

                        AddDataPoint(item.ApplicationIdentity,
                            item.CommandId.Value, command.CommandCpuUsage.Value);

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