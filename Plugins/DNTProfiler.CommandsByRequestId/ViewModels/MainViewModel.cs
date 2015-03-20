using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;
using OxyPlot;
using OxyPlot.Series;

namespace DNTProfiler.CommandsByRequestId.ViewModels
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

            GuiModelData.PlotModel.Title = "Commands By Request IDs";
            AddXAxis("Request ID");
            AddYAxis("Commands Count");
            InitPlotModelEvents();
            InitUpdatePlotInterval();
        }

        protected override string GetCustomTooltip(TrackerHitResult hitTestResult)
        {
            var lineSeries = hitTestResult.Series as LineSeries;
            if (lineSeries == null)
                return string.Empty;

            if (GuiModelData.SelectedApplicationIdentity == null)
                return string.Empty;

            var requestId = getSelectedRequestId(hitTestResult.Index, lineSeries);
            var firstCommand = PluginContext.ProfilerData.Commands
                                    .FirstOrDefault(command => command.HttpInfo.HttpContextCurrentId == requestId &&
                                           command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity));


            return firstCommand == null ? string.Empty : string.Format("URL: {0}", firstCommand.HttpInfo.Url);
        }

        protected override void ShowNearestSelectedPointCommands(AppIdentity identity, LineSeries lineSeries, OxyMouseDownEventArgs args)
        {
            GuiModelData.SelectedApplicationIdentity = identity;
            var requestId = getSelectedRequestId(args.HitTestResult.Index, lineSeries);
            showCommands(requestId);
            setSelectedPointInfo(lineSeries, args);
        }

        private void setSelectedPointInfo(LineSeries lineSeries, OxyMouseDownEventArgs args)
        {
            var firstCommand = GuiModelData.RelatedCommands.FirstOrDefault();
            if (firstCommand == null)
                return;

            SelectedPointInfo =
                string.Format("{0}{1}URL: {2}",
                GetSelectedPointInfo(lineSeries, lineSeries.Points[(int)Math.Round(args.HitTestResult.Index)]),
                Environment.NewLine, firstCommand.HttpInfo.Url);
        }

        private static int getSelectedRequestId(double index, LineSeries lineSeries)
        {
            var indexOfNearestPoint = (int)Math.Round(index);
            var point = lineSeries.Points[indexOfNearestPoint];
            var requestId = (int)point.X;
            return requestId;
        }

        private void Command_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Command item in e.NewItems)
                    {
                        var xValue = item.HttpInfo.HttpContextCurrentId;
                        if (xValue == null)
                            continue;

                        var pointInfo = FindDataPointInfo(item.ApplicationIdentity, xValue.Value);
                        if (pointInfo.Index == -1)
                        {
                            AddDataPoint(item.ApplicationIdentity, xValue.Value, 1);
                            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                            CallbacksManagerBase.UpdateAppIdentityNotificationsCount(item);
                        }
                        else
                        {
                            var newY = pointInfo.Point.Y + 1;
                            pointInfo.LineSeries.Points[pointInfo.Index] = new DataPoint(pointInfo.Point.X, newY);

                            UpdateYMax(newY);
                        }
                        HaveNewPoints = true;
                    }
                    break;
            }
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += Command_CollectionChanged;
        }

        private void showCommands(int requestId)
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            var commands = PluginContext.ProfilerData.Commands
                .Where(command => command.HttpInfo.HttpContextCurrentId == requestId &&
                                  command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .OrderBy(command => command.AtDateTime)
                .ToList();

            GuiModelData.RelatedCommands = new ObservableCollection<Command>(commands);

            CallbacksManagerBase.ActivateRelatedStackTraces();
        }
    }
}