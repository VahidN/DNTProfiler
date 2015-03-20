using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.ByMethods.Core;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByMethods.ViewModels
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
                    _callbacksManager.ShowSelectedApplicationIdentityStackTraces();
                    break;
                case "SelectedStackTrace":
                    _callbacksManager.ShowSelectedStackTraceRelatedCommands();
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
                return GuiModelData.RelatedStackTraces.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.StackTraces.CollectionChanged += StackTraces_CollectionChanged;
        }

        private void StackTraces_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CallingMethodStackTrace item in e.NewItems)
                    {
                        _callbacksManager.ManageStackTraces(item);
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