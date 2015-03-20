using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.MultipleContextPerRequest.Core;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.MultipleContextPerRequest.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        private readonly CallbacksManager _callbacksManager;

        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            setActions();
            setGuiModel();
            _callbacksManager = new CallbacksManager(PluginContext, GuiModelData);
            setEvenets();
        }

        private void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedApplicationIdentity":
                    _callbacksManager.ShowSelectedApplicationIdentityRelatedTrafficWebRequests();
                    break;
                case "SelectedContext":
                    _callbacksManager.ShowSelectedContextRelatedCommands();
                    break;
                case "SelectedTrafficWebRequest":
                    _callbacksManager.ShowRelatedCommandsOfSelectedTrafficWebRequest();
                    break;
                case "SelectedExecutedCommand":
                    _callbacksManager.ShowSelectedCommandRelatedStackTraces();
                    break;
            }
        }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
                _callbacksManager.Reset();
            };

            PluginContext.GetResults = () =>
            {
                return GuiModelData.RelatedTrafficWebRequests.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.TrafficWebRequests.CollectionChanged += TrafficWebRequests_CollectionChanged;
        }

        void TrafficWebRequests_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TrafficWebRequest item in e.NewItems)
                    {
                        _callbacksManager.ManageTrafficWebRequest(item);
                    }
                    break;
            }
        }

        private void setGuiModel()
        {
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }
    }
}