using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.NonDisposedContexts.Core;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.NonDisposedContexts.ViewModels
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
                                _callbacksManager.AddNewOpenedConnections(item);
                                break;
                            case CommandConnectionType.Disposed:
                                _callbacksManager.RemoveDisposedConnection(item);
                                break;
                        }
                    }
                    break;
            }
        }

        private void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedApplicationIdentity":
                    _callbacksManager.ShowSelectedApplicationIdentityLocalConnections();
                    break;
                case "SelectedConnection":
                    _callbacksManager.ShowSelectedConnectionRelatedCommands();
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
                return PluginContext.ProfilerData.Contexts.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Connections.CollectionChanged += Connections_CollectionChanged;
        }

        private void setGuiModel()
        {
            GuiModelData = new GuiModelBase();
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }
    }
}