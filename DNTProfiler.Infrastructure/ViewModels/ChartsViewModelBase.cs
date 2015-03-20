using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DNTProfiler.Infrastructure.ViewModels
{
    public class ChartsViewModelBase : MainViewModelBase
    {
        protected readonly Stopwatch Stopwatch = new Stopwatch();
        protected readonly LinearAxis XAxis = new LinearAxis();
        protected readonly LinearAxis YAxis = new LinearAxis();
        protected CallbacksManagerBase CallbacksManagerBase;
        protected IPlotController Controller;
        protected bool HaveNewPoints;
        protected long LastUpdateMilliseconds;
        protected Dictionary<AppIdentity, LineSeries> LineSeries = new Dictionary<AppIdentity, LineSeries>();
        protected int RefreshEveryMilliseconds = 2000;
        protected double XMax;
        protected double YMax;

        private string _selectedPointInfo;

        public ChartsViewModelBase(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            CallbacksManagerBase = new CallbacksManagerBase(PluginContext, GuiModelData);
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
            CustomTooltipProvider = new CustomTooltipProvider { GetCustomTooltip = GetCustomTooltip };
        }

        public CustomTooltipProvider CustomTooltipProvider { set; get; }

        public IPlotController PlotController
        {
            get
            {
                if (Controller == null)
                {
                    // show tracker with mouse move
                    Controller = new PlotController();
                    Controller.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                }
                return Controller;
            }
        }

        public string SelectedPointInfo
        {
            get { return _selectedPointInfo; }
            set
            {
                _selectedPointInfo = value;
                NotifyPropertyChanged(() => SelectedPointInfo);
            }
        }

        protected virtual void AddDataPoint(AppIdentity identity, double x, double y)
        {
            LineSeries lineSeries;
            if (!LineSeries.TryGetValue(identity, out lineSeries))
            {
                lineSeries = CreateLineSeries(identity);
            }

            lineSeries.Points.Add(new DataPoint(x, y));

            UpdateXMax(x);
            UpdateYMax(y);
            HaveNewPoints = true;
        }

        protected virtual void AddXAxis(string title, double maximumPadding = 1, double minimumPadding = 1)
        {
            XAxis.Minimum = 0;
            XAxis.MaximumPadding = maximumPadding;
            XAxis.MinimumPadding = minimumPadding;
            XAxis.Position = AxisPosition.Bottom;
            XAxis.Title = title;
            XAxis.MajorGridlineStyle = LineStyle.Solid;
            XAxis.MinorGridlineStyle = LineStyle.Dot;
            GuiModelData.PlotModel.Axes.Add(XAxis);
        }

        protected virtual void AddYAxis(string title, double maximumPadding = 1, double minimumPadding = 1)
        {
            YAxis.Minimum = 0;
            YAxis.Title = title;
            YAxis.MaximumPadding = maximumPadding;
            YAxis.MinimumPadding = minimumPadding;
            YAxis.MajorGridlineStyle = LineStyle.Solid;
            YAxis.MinorGridlineStyle = LineStyle.Dot;
            GuiModelData.PlotModel.Axes.Add(YAxis);
        }

        protected override void ApplicationIdentities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (AppIdentity appIdentity in e.NewItems)
                    {
                        CreateLineSeries(appIdentity);
                    }
                    break;
            }
        }

        protected LineSeries CreateLineSeries(AppIdentity appIdentity)
        {
            var identity = new AppIdentity(appIdentity);
            GuiModelData.ApplicationIdentities.Add(identity);

            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                StrokeThickness = 2,
                MarkerSize = 3,
                Title = identity.ProcessName + " [" + identity.AppDomainName + "]"
            };
            lineSeries.MouseDown += (from, args) =>
            {
                if (args.ChangedButton == OxyMouseButton.Left)
                {
                    ShowNearestSelectedPointCommands(identity, lineSeries, args);
                }
            };

            GuiModelData.PlotModel.Series.Add(lineSeries);
            LineSeries.Add(identity, lineSeries);

            return lineSeries;
        }

        protected virtual DataPointInfo FindDataPointInfo(AppIdentity appIdentity, double xAxisId)
        {
            var defaultResult = new DataPointInfo(default(DataPoint), -1, null);

            LineSeries lineSeries;
            if (!LineSeries.TryGetValue(appIdentity, out lineSeries))
                return defaultResult;

            var result = lineSeries.Points.Select((value, index) => new { value, index })
                                          .FirstOrDefault(point => point.value.X.ApproxEquals(xAxisId));
            return result == null ? defaultResult : new DataPointInfo(result.value, result.index, lineSeries);
        }

        /// <summary>
        /// Override it to add a custom tooltip to the chart's tracker
        /// </summary>
        protected virtual string GetCustomTooltip(TrackerHitResult hitTestResult)
        {
            return string.Empty;
        }

        protected virtual string GetSelectedPointInfo(LineSeries lineSeries, DataPoint point)
        {
            return string.Format("{0}{1}{2}: {3}{1}{4}: {5}",
                lineSeries.Title, Environment.NewLine,
                lineSeries.XAxis.Title, point.X,
                lineSeries.YAxis.Title, point.Y);
        }

        protected virtual void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedApplicationIdentity":
                    MakeSelectedApplicationIdentityVisible();
                    break;
                case "SelectedExecutedCommand":
                    CallbacksManagerBase.ShowSelectedCommandRelatedStackTraces();
                    break;
            }
        }

        protected virtual void InitPlotModelEvents()
        {
            GuiModelData.PlotModel.MouseDown += (sender, args) =>
            {
                if (args.ChangedButton == OxyMouseButton.Left && args.ClickCount == 2)
                {
                    foreach (var axis in GuiModelData.PlotModel.Axes)
                        axis.Reset();

                    foreach (var lineSeries in LineSeries)
                        lineSeries.Value.IsVisible = true;

                    GuiModelData.PlotModel.InvalidatePlot(false);
                }
            };
        }

        protected virtual void InitUpdatePlotInterval()
        {
            CompositionTarget.Rendering += (sender, args) =>
            {
                if (Stopwatch.ElapsedMilliseconds > LastUpdateMilliseconds + RefreshEveryMilliseconds && HaveNewPoints)
                {
                    if (YMax > 0 && XMax > 0)
                    {
                        YAxis.Maximum = YMax + 3;
                        XAxis.Maximum = XMax + 1;
                    }

                    GuiModelData.PlotModel.InvalidatePlot(false);

                    HaveNewPoints = false;
                    LastUpdateMilliseconds = Stopwatch.ElapsedMilliseconds;
                }
            };

            Stopwatch.Start();
        }

        protected virtual void MakeSelectedApplicationIdentityVisible()
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
                return;

            foreach (var lineSeries in LineSeries)
            {
                lineSeries.Value.IsVisible = lineSeries.Key.Equals(GuiModelData.SelectedApplicationIdentity);
            }

            GuiModelData.PlotModel.InvalidatePlot(false);
        }

        protected virtual void ResetPlot()
        {
            foreach (var axis in GuiModelData.PlotModel.Axes)
                axis.Reset();

            GuiModelData.PlotModel.Series.Clear();
            LineSeries.Clear();

            XMax = 0;
            YMax = 0;

            GuiModelData.PlotModel.InvalidatePlot(false);
        }

        protected virtual void SetActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
                ResetPlot();
            };

            PluginContext.GetResults = () =>
            {
                return GuiModelData.RelatedCommands.ToFormattedJson();
            };
        }

        protected virtual void ShowNearestSelectedPointCommands(AppIdentity identity, LineSeries lineSeries, OxyMouseDownEventArgs args)
        {
            GuiModelData.SelectedApplicationIdentity = identity;

            var indexOfNearestPoint = (int)Math.Round(args.HitTestResult.Index);
            var point = lineSeries.Points[indexOfNearestPoint];
            var commandId = (int)point.X;
            CallbacksManagerBase.ShowSelectedCommand(commandId);

            SelectedPointInfo = GetSelectedPointInfo(lineSeries, point);
        }

        protected virtual void UpdateXMax(double x)
        {
            if (x > XMax)
            {
                XMax = x;
            }
        }

        protected virtual void UpdateYMax(double y)
        {
            if (y > YMax)
            {
                YMax = y;
            }
        }
    }
}