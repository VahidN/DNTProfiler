using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.ByTransactions.Core;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByTransactions.ViewModels
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

        private void Transactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandTransaction item in e.NewItems)
                    {
                        switch (item.TransactionType)
                        {
                            case CommandTransactionType.Began:
                                _callbacksManager.TransactionBegan(item);
                                break;
                            case CommandTransactionType.Committed:
                                _callbacksManager.TransactionCommitted(item);
                                break;
                            case CommandTransactionType.RolledBack:
                                _callbacksManager.TransactionRolledBack(item);
                                break;
                            case CommandTransactionType.Disposed:
                                _callbacksManager.TransactionDisposed(item);
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
                    _callbacksManager.ShowSelectedApplicationIdentityTransactions();
                    break;
                case "SelectedTransaction":
                    _callbacksManager.ShowSelectedTransactionRelatedCommands();
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
            };

            PluginContext.GetResults = () =>
            {
                return GuiModelData.RelatedCommands.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Transactions.CollectionChanged += Transactions_CollectionChanged;
        }

        private void setGuiModel()
        {
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }
    }
}