using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;
using DNTProfiler.RawLogger.Models;
using Newtonsoft.Json;

namespace DNTProfiler.RawLogger.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            ThisGuiModelData = new MainGuiModel();
            ThisGuiModelData.PropertyChanged += ThisGuiModelData_PropertyChanged;
            setActions();
            setEvenets();
        }

        public MainGuiModel ThisGuiModelData { set; get; }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
                ThisGuiModelData.ProfilerItems.Clear();
            };

            PluginContext.GetResults = () =>
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("[");
                foreach (var info in ThisGuiModelData.ProfilerItems)
                {
                    if (string.IsNullOrWhiteSpace(info.JsonContent))
                    {
                        info.JsonContent = info.ToFormattedJson(TypeNameHandling.Objects);
                    }

                    stringBuilder.AppendLine(info.JsonContent);
                    stringBuilder.Append(",");
                    stringBuilder.AppendLine(Environment.NewLine);
                }
                stringBuilder.AppendLine("]");
                return stringBuilder.ToString();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += (sender, args) => { showItems(args); };
            PluginContext.ProfilerData.Connections.CollectionChanged += (sender, args) => { showItems(args); };
            PluginContext.ProfilerData.Results.CollectionChanged += (sender, args) => { showItems(args); };
            PluginContext.ProfilerData.Transactions.CollectionChanged += (sender, args) => { showItems(args); };
        }

        private void showItems(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (BaseInfo item in e.NewItems)
                    {
                        ThisGuiModelData.ProfilerItems.Add(item);
                        PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                    }
                    break;
            }
        }

        void ThisGuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedProfilerItem":
                    var info = ThisGuiModelData.SelectedProfilerItem;
                    if (info == null) return;
                    if (string.IsNullOrWhiteSpace(info.JsonContent))
                    {
                        info.JsonContent = info.ToFormattedJson(TypeNameHandling.Objects);
                    }
                    break;
            }
        }
    }
}