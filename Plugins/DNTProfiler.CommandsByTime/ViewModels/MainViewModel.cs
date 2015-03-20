using System;
using System.Linq;
using System.Collections.Specialized;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using DNTProfiler.Common.Models;

namespace DNTProfiler.CommandsByTime.ViewModels
{
    public class MainViewModel : ChartsViewModelBase
    {
        private DateTime? _epoch;

        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            SetActions();
            setEvenets();

            GuiModelData.PlotModel.Title = "Commands Time Line";
            AddXAxis("Time (seconds)");
            AddYAxis("Commands Count");
            InitPlotModelEvents();
            InitUpdatePlotInterval();
        }

        protected override void SetActions()
        {
            _epoch = null;
            base.SetActions();
        }

        protected override void ShowNearestSelectedPointCommands(AppIdentity identity, LineSeries lineSeries, OxyMouseDownEventArgs args)
        {
            GuiModelData.SelectedApplicationIdentity = identity;

            var indexOfNearestPoint = (int)Math.Round(args.HitTestResult.Index);
            var point = lineSeries.Points[indexOfNearestPoint];
            var time = (long)point.X;
            showCommands(time);

            SelectedPointInfo = GetSelectedPointInfo(lineSeries, point);
        }

        private void Command_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Command item in e.NewItems)
                    {
                        if (_epoch == null)
                        {
                            _epoch = item.AtDateTime;
                        }
                        var xValue = dateTimeDiff(item.AtDateTime);

                        var pointInfo = FindDataPointInfo(item.ApplicationIdentity, xValue);
                        if (pointInfo.Index == -1)
                        {
                            AddDataPoint(item.ApplicationIdentity, xValue, 1);
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

        private long dateTimeDiff(DateTime dateTime)
        {
            return _epoch == null ? 0 : (long)(dateTime - _epoch.Value).TotalSeconds;
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += Command_CollectionChanged;
        }

        private void showCommands(long time)
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            var commands = PluginContext.ProfilerData.Commands
                .Where(command => dateTimeDiff(command.AtDateTime) == time &&
                                  command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .OrderBy(command => command.AtDateTime)
                .ToList();

            GuiModelData.RelatedCommands = new ObservableCollection<Command>(commands);

            CallbacksManagerBase.ActivateRelatedStackTraces();
        }
    }
}