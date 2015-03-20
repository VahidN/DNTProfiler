using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.InCorrectNullComparisons.ViewModels
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
                    foreach (Command command in e.NewItems)
                    {
                        var visitor = new NullComparisonVisitor();
                        foreach (var parameter in command.Parameters.Where(parameter => parameter.Value == "null"))
                        {
                            visitor.NullVariableNames.Add(parameter.Name);
                        }
                        _callbacksManager.RunAnalysisVisitorOnCommand(visitor, command);
                    }
                    break;
            }
        }

        private void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedApplicationIdentity":
                    _callbacksManager.ShowSelectedApplicationIdentityLocalCommands();
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
            PluginContext.ProfilerData.Commands.CollectionChanged += Commands_CollectionChanged;
        }

        private void setGuiModel()
        {
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }
    }
}