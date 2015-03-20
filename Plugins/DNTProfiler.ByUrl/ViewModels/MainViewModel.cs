using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.ByUrl.Core;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByUrl.ViewModels
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
                    _callbacksManager.ShowSelectedApplicationIdentityTrafficUrls();
                    break;
                case "SelectedTrafficUrl":
                    _callbacksManager.ShowSelectedTrafficUrlRelatedCommands();
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
                return GuiModelData.RelatedTrafficUrls.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.TrafficUrls.CollectionChanged += TrafficUrls_CollectionChanged;
        }

        private void setGuiModel()
        {
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }

        private void TrafficUrls_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TrafficUrl item in e.NewItems)
                    {
                        _callbacksManager.ManageTrafficUrls(item);
                    }
                    break;
            }
        }
    }
}