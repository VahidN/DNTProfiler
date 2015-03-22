using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Toolkit;
using DNTProfiler.Dumper.Core;
using DNTProfiler.Dumper.Models;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;
using Newtonsoft.Json;

namespace DNTProfiler.Dumper.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        private readonly string _settingsPath = Path.Combine(AppMessenger.ExecutablePathDir, "Plugins", "DNTProfiler.Dumper.json");
        private DirectoryMonitor _directoryMonitor;
        private JsonLogger _jsonLogger;

        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            manageAppExit();
            initThisGuiModelData();
            setActions();
            setEvenets();
            setCommands();
            setDirectoryMonitor();
        }

        public DelegateCommand<string> DoOpenContainingFolder { set; get; }

        public DelegateCommand<FileInfo> DoOpenJsonFile { set; get; }

        public DelegateCommand<string> DoReloadFilesList { set; get; }

        public DelegateCommand<FileInfo> DoReplayJsonFile { set; get; }

        public MainGuiModel ThisGuiModelData { set; get; }

        private void createDumperDirectory()
        {
            if (!Directory.Exists(ThisGuiModelData.DumperSettings.DumperDirectory))
                Directory.CreateDirectory(ThisGuiModelData.DumperSettings.DumperDirectory);
        }

        private void disposeLogger()
        {
            if (_jsonLogger != null)
            {
                _jsonLogger.Dispose();
                _jsonLogger = null;
            }

            if (_directoryMonitor != null)
            {
                _directoryMonitor.Dispose();
                _directoryMonitor = null;
            }
        }

        void DumperSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DumperDirectory":
                    setDirectoryMonitor();
                    resetJsonLogger();
                    showFilesList();
                    break;
            }
            saveSettings();
        }

        private void initThisGuiModelData()
        {
            var settings = JsonHelper.DeserializeFromFile<DumperSettings>(_settingsPath);
            ThisGuiModelData = new MainGuiModel();

            if (settings == null)
            {
                ThisGuiModelData.DumperSettings.DumperDirectory = Path.Combine(AppMessenger.ExecutablePathDir, "Dumps");
            }
            else
            {
                ThisGuiModelData.DumperSettings = settings;
            }
            saveSettings();

            ThisGuiModelData.DumperSettings.PropertyChanged += DumperSettings_PropertyChanged;
        }

        private void logItemsToFile(NotifyCollectionChangedEventArgs e)
        {
            if (!ThisGuiModelData.DumperSettings.IsActive)
                return;

            if (_jsonLogger == null)
            {
                resetJsonLogger();
                showFilesList();
            }

            if (_jsonLogger == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.Cast<BaseInfo>().Where(item => _jsonLogger != null))
                    {
                        _jsonLogger.WriteJsonObject(item.JsonContent);
                    }
                    break;
            }
        }

        private void manageAppExit()
        {
            Application.Current.Exit += (sender, args) => { disposeLogger(); };
            Application.Current.SessionEnding += (sender, args) => { disposeLogger(); };
        }

        private void notifyPluginsHost()
        {
            if (PluginContext.NotifyPluginsHost == null) return;
            PluginContext.NotifyPluginsHost(NotificationType.Reset, ThisGuiModelData.Files.Count);
        }

        private void resetJsonLogger()
        {
            disposeLogger();

            var fileName = string.Format("log-{0}.json", DateTime.Now.ToString("yyyy-MM-dd[hh-mm-ss]"));
            var path = Path.Combine(ThisGuiModelData.DumperSettings.DumperDirectory, fileName);
            _jsonLogger = new JsonLogger(path);
        }

        private void saveSettings()
        {
            JsonHelper.SerializeToFile(_settingsPath, ThisGuiModelData.DumperSettings,
                new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
                disposeLogger();
            };

            PluginContext.MainWindowIsLoaded = () =>
            {
                showFilesList();
            };

            PluginContext.GetResults = () =>
            {
                var stringBuilder = new StringBuilder();
                foreach (var fileInfo in ThisGuiModelData.Files)
                {
                    stringBuilder.AppendLine(fileInfo.FullName);
                }
                return stringBuilder.ToString();
            };
        }

        private void setCommands()
        {
            DoOpenJsonFile = new DelegateCommand<FileInfo>(info =>
            {
                if (info == null) return;
                Process.Start("notepad", info.FullName);
            }, info => true);

            DoReplayJsonFile = new DelegateCommand<FileInfo>(info =>
            {
                if (info == null) return;
                new JsonLoader(PluginContext, ThisGuiModelData).Start(info.FullName);
            }, info => true);

            DoReloadFilesList = new DelegateCommand<string>(info =>
            {
                showFilesList();
            }, info => true);

            DoOpenContainingFolder = new DelegateCommand<string>(info =>
            {
                Process.Start(ThisGuiModelData.DumperSettings.DumperDirectory);
            }, info => true);
        }

        private void setDirectoryMonitor()
        {
            if (_directoryMonitor != null)
                _directoryMonitor.Dispose();

            createDumperDirectory();

            _directoryMonitor = new DirectoryMonitor(ThisGuiModelData.DumperSettings.DumperDirectory, "*.json")
            {
                OnFileSystemChanged = file =>
                {
                    showFilesList();
                }
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Commands.CollectionChanged += (sender, args) => { logItemsToFile(args); };
            PluginContext.ProfilerData.Connections.CollectionChanged += (sender, args) => { logItemsToFile(args); };
            PluginContext.ProfilerData.Results.CollectionChanged += (sender, args) => { logItemsToFile(args); };
            PluginContext.ProfilerData.Transactions.CollectionChanged += (sender, args) => { logItemsToFile(args); };
        }

        private void showFilesList()
        {
            createDumperDirectory();
            var files = new DirectoryInfo(ThisGuiModelData.DumperSettings.DumperDirectory).GetFiles("*.json").OrderByDescending(x => x.LastWriteTime);
            ThisGuiModelData.Files = new ObservableCollection<FileInfo>(files);

            notifyPluginsHost();
        }
    }
}