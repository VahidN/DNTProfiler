using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.Common.Converters;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.EFTrafficLogger.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        private readonly CallbacksManagerBase _callbacksManager;

        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            setActions();
            setGuiModel();
            _callbacksManager = new CallbacksManagerBase(PluginContext, GuiModelData);
            setEvenets();
        }

        private void Commands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Command item in e.NewItems)
                    {
                        _callbacksManager.UpdateApplicationIdentities(item);
                        _callbacksManager.AddStackTrace(item);
                        _callbacksManager.AddTrafficUrl(item);
                        _callbacksManager.CreateOrUpdateContext(item);
                        _callbacksManager.UpdateConnectionCommandsCount(item);
                        _callbacksManager.UpdateTransactionsCommandsCount(item);
                        _callbacksManager.UpdateCommandsStatistics(item);
                        _callbacksManager.UpdateThreadsList(item);
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
                        _callbacksManager.UpdateApplicationIdentities(item);
                        _callbacksManager.AddStackTrace(item);
                        _callbacksManager.AddTrafficUrl(item);

                        switch (item.Type)
                        {
                            case CommandConnectionType.Opened:
                                _callbacksManager.CreateOrUpdateContext(item);
                                break;
                            case CommandConnectionType.Closed:
                                _callbacksManager.UpdateConnectionsClosedTime(item);
                                break;
                            case CommandConnectionType.Disposed:
                                _callbacksManager.SetConnectionAndContexAsDisposed(item);
                                break;
                        }

                        _callbacksManager.UpdateThreadsList(item);
                    }
                    break;
            }
        }

        private void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedApplicationIdentity":
                    _callbacksManager.ShowSelectedApplicationIdentityContexts();
                    break;
                case "SelectedContext":
                    _callbacksManager.ShowSelectedContextRelatedConnections();
                    break;
                case "SelectedConnection":
                    _callbacksManager.ShowSelectedConnectionRelatedCommands();
                    break;
                case "SelectedExecutedCommand":
                    _callbacksManager.ShowSelectedCommandRelatedStackTraces();
                    break;
            }
        }

        private void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandResult item in e.NewItems)
                    {
                        _callbacksManager.UpdateApplicationIdentities(item);
                        _callbacksManager.AddStackTrace(item);
                        _callbacksManager.AddTrafficUrl(item);
                        _callbacksManager.UpdateThreadsList(item);
                        _callbacksManager.FinishRelatedCommand(item);
                        _callbacksManager.UpdateNumberOfExceptions(item);
                        _callbacksManager.UpdateRelatedCommandsRowCount(item);
                    }
                    break;
            }
        }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
            };

            PluginContext.GetResults = () =>
            {
                return PluginContext.ProfilerData.Contexts.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += Commands_CollectionChanged;
            PluginContext.ProfilerData.Connections.CollectionChanged += Connections_CollectionChanged;
            PluginContext.ProfilerData.Results.CollectionChanged += Results_CollectionChanged;
            PluginContext.ProfilerData.Transactions.CollectionChanged += Transactions_CollectionChanged;
        }

        private void setGuiModel()
        {
            GuiModelData = new GuiModelBase();
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }

        private void Transactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandTransaction item in e.NewItems)
                    {
                        _callbacksManager.UpdateApplicationIdentities(item);
                        _callbacksManager.AddStackTrace(item);
                        _callbacksManager.AddTrafficUrl(item);
                        _callbacksManager.UpdateThreadsList(item);

                        switch (item.TransactionType)
                        {
                            case CommandTransactionType.Began:
                                _callbacksManager.UpdateNewConnectionsTransactionIds(item);
                                _callbacksManager.UpdateNumberOfTransactions(item);
                                break;
                            case CommandTransactionType.Committed:
                                _callbacksManager.UpdateNumberOfCommittedTransactions(item);
                                break;
                            case CommandTransactionType.RolledBack:
                                _callbacksManager.UpdateNumberOfRolledBackTransactions(item);
                                break;
                        }
                    }
                    break;
            }
        }
    }
}