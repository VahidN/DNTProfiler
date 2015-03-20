using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DNTProfiler.Common.ClipboardUtils;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Toolkit;
using DNTProfiler.Core;
using DNTProfiler.Models;
using DNTProfiler.Services;
using System.Windows;
using DNTProfiler.Common.Models;
using System.Threading.Tasks;
using DNTProfiler.Common.Logger;

namespace DNTProfiler.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly ICommonDialogsService _commonDialogsService;
        private readonly IDialogManager _dialogManager;
        private readonly ProfilerData _profilerData = new ProfilerData();
        private readonly SelfHostConfig _selfHostConfig = new SelfHostConfig();
        private bool _isStated;
        private LoadPlugins _loadPlugins;

        public MainWindowViewModel(ICommonDialogsService commonDialogsService, IDialogManager dialogManager)
        {
            _commonDialogsService = commonDialogsService;
            _dialogManager = dialogManager;

            if (Designer.IsInDesignModeStatic)
                return;

            manageAppExit();
            registerMessengers();
            GuiModelData = new GuiModel();
            loadPlugins();
            loadConfig();
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
            setCommands();
            doStart(string.Empty);
        }

        public DelegateCommand<string> DoClearLogs { set; get; }

        public DelegateCommand<string> DoCopy { set; get; }

        public DelegateCommand<string> DoSaveResults { set; get; }

        public DelegateCommand<string> DoStart { set; get; }

        public DelegateCommand<string> DoStop { set; get; }

        public DelegateCommand<string> DoLoadMainWindowCommand { set; get; }

        public GuiModel GuiModelData { set; get; }

        private void activatePluginTabItem()
        {
            if (GuiModelData.SelectedPlugin != null)
                GuiModelData.SelectedTabIndex = GuiModelData.SelectedPlugin.Id;
        }

        void currentExit(object sender, ExitEventArgs e)
        {
            _selfHostConfig.Stop();
        }

        void currentSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            _selfHostConfig.Stop();
        }

        void doCopy(string data)
        {
            var plugin = getSelectedTabPlugin();
            if (plugin != null)
            {
                plugin.ProfilerPlugin.GetResults().ClipboardSetText();
            }
        }

        void doSave(string data)
        {
            var path = _commonDialogsService.ShowSaveFileDialogGetPath();
            if (string.IsNullOrWhiteSpace(path))
                return;

            var plugin = getSelectedTabPlugin();
            if (plugin != null)
            {
                File.WriteAllText(path, plugin.ProfilerPlugin.GetResults());
            }
        }

        private void doSearch()
        {
            if (string.IsNullOrWhiteSpace(GuiModelData.PluginSearchText))
            {
                GuiModelData.Plugins = _loadPlugins.Plugins;
            }
            else
            {
                GuiModelData.Plugins =
                    _loadPlugins.Plugins.Where(
                        x =>
                            x.ProfilerPlugin.PluginMetadata.PluginName.ToLowerInvariant().Contains(
                                    GuiModelData.PluginSearchText.ToLowerInvariant())).ToList();
            }
        }

        void doStart(string data)
        {
            _selfHostConfig.OpenWait(GuiModelData.ServerUri, GuiModelData.AllowRemoteConnections);
            _isStated = true;
        }

        void doStop(string data)
        {
            _selfHostConfig.Stop();
            _isStated = false;
        }

        private Plugin getSelectedTabPlugin()
        {
            return GuiModelData.Plugins.FirstOrDefault(x => x.Id == GuiModelData.SelectedTabIndex);
        }

        void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedTabIndex":
                    setSelectedPlugin();
                    break;
                case "PluginSearchText":
                    doSearch();
                    break;

                case "SelectedPlugin":
                    setPluginAuthorInfo();
                    activatePluginTabItem();
                    break;

                case "AllowRemoteConnections":
                case "ServerUri":
                    saveConfig();
                    break;
            }
        }

        private void loadConfig()
        {
            GuiModelData.ServerUri = ConfigSetGet.GetConfigData("ServerUri");
            GuiModelData.AllowRemoteConnections = bool.Parse(ConfigSetGet.GetConfigData("AllowRemoteConnections"));
        }

        private void loadPlugins()
        {
            GuiModelData.IsBusy = true;
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                _loadPlugins = new LoadPlugins(_profilerData, GuiModelData, _dialogManager, _commonDialogsService);
                _loadPlugins.Start();
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                    {
                        task.Exception.Flatten().Handle(ex =>
                        {
                            new ExceptionLogger().LogExceptionToFile(ex, AppMessenger.LogFile);
                            return false;
                        });
                    }

                    GuiModelData.IsBusy = false;
                    return;
                }

                GuiModelData.Plugins = _loadPlugins.Plugins;
                _loadPlugins.NotifyMainWindowIsLoaded();

                setTheSelectedPlugin();
                GuiModelData.IsBusy = false;
            }, taskScheduler);
        }

        private void manageAppExit()
        {
            Application.Current.Exit += currentExit;
            Application.Current.SessionEnding += currentSessionEnding;
        }

        private void registerMessengers()
        {
            AppMessenger.Messenger.Register<CommandTransaction>("AddCommandTransaction",
                log => _profilerData.Transactions.Add(log));

            AppMessenger.Messenger.Register<Command>("AddCommand",
                log => _profilerData.Commands.Add(log));

            AppMessenger.Messenger.Register<CommandResult>("AddCommandResult",
                log => _profilerData.Results.Add(log));

            AppMessenger.Messenger.Register<CommandConnection>("AddCommandConnection",
                log => _profilerData.Connections.Add(log));
        }

        private void saveConfig()
        {
            ConfigSetGet.SetConfigData("ServerUri", GuiModelData.ServerUri);
            ConfigSetGet.SetConfigData("AllowRemoteConnections", GuiModelData.AllowRemoteConnections.ToString());
        }

        private void setCommands()
        {
            DoStart = new DelegateCommand<string>(doStart, _ => !_isStated);
            DoStop = new DelegateCommand<string>(doStop, _ => true);
            DoCopy = new DelegateCommand<string>(doCopy, _ => true);
            DoClearLogs = new DelegateCommand<string>(info => _loadPlugins.ResetAll(0), _ => true);
            DoSaveResults = new DelegateCommand<string>(doSave, _ => true);
            DoLoadMainWindowCommand = new DelegateCommand<string>(_ => { }, _ => true);
        }

        private void setTheSelectedPlugin()
        {
            if (GuiModelData.Plugins != null)
                GuiModelData.SelectedPlugin = GuiModelData.Plugins.FirstOrDefault(x => x.Header == "By Context");
        }

        private void setPluginAuthorInfo()
        {
            var value = GuiModelData.SelectedPlugin;
            if (value == null)
                return;

            GuiModelData.PluginAuthorInfo = string.Format("{0}{1}By {2}, {3}",
                value.ProfilerPlugin.PluginMetadata.PluginDescription,
                Environment.NewLine,
                value.ProfilerPlugin.PluginAuthor.Name,
                value.ProfilerPlugin.PluginAuthor.WebSiteUrl);
        }

        private void setSelectedPlugin()
        {
            var plugin = getSelectedTabPlugin();
            if (plugin != null)
            {
                GuiModelData.SelectedPlugin = plugin;
            }
        }
    }
}