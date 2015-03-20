using System;
using System.Collections.Specialized;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;
using OxyPlot;
using OxyPlot.Series;

namespace DNTProfiler.ConnectionsByRoundtrips.ViewModels
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

            GuiModelData.PlotModel.Title = "Connections By Roundtrips";
            AddXAxis("Connection (Id)");
            AddYAxis("Commands Count");
            InitPlotModelEvents();
            InitUpdatePlotInterval();
        }

        protected override void ShowNearestSelectedPointCommands(AppIdentity identity, LineSeries lineSeries, OxyMouseDownEventArgs args)
        {
            GuiModelData.SelectedApplicationIdentity = identity;

            var indexOfNearestPoint = (int)Math.Round(args.HitTestResult.Index);
            var point = lineSeries.Points[indexOfNearestPoint];
            var connectionId = (int)point.X;
            CallbacksManagerBase.ShowAllCommandsWithSameConnectionId(connectionId);

            SelectedPointInfo = GetSelectedPointInfo(lineSeries, point);
        }

        private void Command_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Command item in e.NewItems)
                    {
                        if (item.ConnectionId == null)
                            continue;

                        var pointInfo = FindDataPointInfo(item.ApplicationIdentity, item.ConnectionId.Value);
                        if (pointInfo.Index == -1)
                            continue;

                        var newY = pointInfo.Point.Y + 1;
                        pointInfo.LineSeries.Points[pointInfo.Index] = new DataPoint(pointInfo.Point.X, newY);

                        UpdateYMax(newY);
                        HaveNewPoints = true;
                    }
                    break;
            }
        }

        private void Connections_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandConnection item in e.NewItems)
                    {
                        switch (item.Type)
                        {
                            case CommandConnectionType.Opened:
                                {
                                    if (item.ConnectionId == null)
                                        continue;

                                    if (isConnectionDataPointAdded(item))
                                        continue;

                                    AddDataPoint(item.ApplicationIdentity, item.ConnectionId.Value, 0);
                                    PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                                    CallbacksManagerBase.UpdateAppIdentityNotificationsCount(item);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private bool isConnectionDataPointAdded(BaseInfo item)
        {
            if (item.ConnectionId == null)
                return false;

            var point = FindDataPointInfo(item.ApplicationIdentity, item.ConnectionId.Value);
            return point.Index != -1;
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += Command_CollectionChanged;
            PluginContext.ProfilerData.Connections.CollectionChanged += Connections_CollectionChanged;
        }
    }
}