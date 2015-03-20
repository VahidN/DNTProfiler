using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.Core
{
    public class LoadPlugins
    {
        private readonly GuiModel _guiModelData;
        private readonly IDialogManager _dialogManager;
        private readonly ICommonDialogsService _commonDialogsService;
        private readonly ProfilerData _profilerData;
        private IList<Plugin> _plugins = new List<Plugin>();

        public LoadPlugins(ProfilerData profilerData,
                           GuiModel guiModelData,
                           IDialogManager dialogManager,
                           ICommonDialogsService commonDialogsService)
        {
            _profilerData = profilerData;
            _guiModelData = guiModelData;
            _dialogManager = dialogManager;
            _commonDialogsService = commonDialogsService;
        }

        public IList<Plugin> Plugins
        {
            get { return _plugins; }
        }

        public void NotifyMainWindowIsLoaded()
        {
            if (_plugins == null) return;
            foreach (var plugin in _plugins.Where(plugin => plugin.ProfilerPlugin.MainWindowIsLoaded != null))
            {
                plugin.ProfilerPlugin.MainWindowIsLoaded();
            }
        }

        public void ResetAll(int value)
        {
            foreach (var plugin in _plugins)
            {
                plugin.ProfilerPlugin.Reset();
            }

            _profilerData.ClearAll();

            foreach (var plugin in _plugins)
            {
                plugin.NotificationsCount = value;
            }
        }

        public void Start()
        {
            var path = Path.Combine(AppMessenger.Path, "Plugins");
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path + " not found.");

            loadPlugins(path);
            fixIds();
        }

        private static bool isAlreadyLoaded(string pluginPath)
        {
            var assemblyName = AssemblyName.GetAssemblyName(pluginPath);
            return AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                AssemblyName.ReferenceMatchesDefinition(assemblyName, assembly.GetName()));
        }

        private void fixIds()
        {
            _plugins = _plugins.OrderBy(plugin => plugin.Category).ThenBy(plugin => plugin.Header).ToList();
            var index = 0;
            foreach (var plugin in _plugins)
            {
                plugin.Id = index++;
            }
        }

        private void loadPlugins(string path)
        {
            foreach (var pluginPath in Directory.GetFiles(path, "*.dll"))
            {
                if (isAlreadyLoaded(pluginPath))
                    continue;

                var pluginAssembly = Assembly.LoadFrom(pluginPath);

                var profilerPlugin = pluginAssembly.GetTypes()
                    .FirstOrDefault(type => typeof(ProfilerPluginBase).IsAssignableFrom(type));
                if (profilerPlugin == null)
                    continue;

                var pluginInstance = Activator.CreateInstance(profilerPlugin) as ProfilerPluginBase;
                if (pluginInstance == null)
                    continue;

                pluginInstance.ProfilerData = _profilerData;

                var plugin = new Plugin
                {
                    PluginAssembly = pluginAssembly,
                    ProfilerPlugin = pluginInstance
                };

                if (plugin.Content == null)
                    continue;

                pluginInstance.CustomDialogsService = _dialogManager;
                pluginInstance.CommonDialogsService = _commonDialogsService;

                pluginInstance.NotifyPluginsHost = (type, value) =>
                {
                    notifyPluginsHost(type, value, plugin);
                };
                _plugins.Add(plugin);
            }
        }

        private void notifyPluginsHost(NotificationType type, int value, Plugin plugin)
        {
            switch (type)
            {
                case NotificationType.ResetAll:
                    ResetAll(value);
                    break;
                case NotificationType.Reset:
                    plugin.NotificationsCount = value;
                    break;
                case NotificationType.Update:
                    plugin.NotificationsCount += value;
                    break;
                case NotificationType.ShowBusyIndicator:
                    _guiModelData.IsBusy = true;
                    break;
                case NotificationType.HideBusyIndicator:
                    _guiModelData.IsBusy = false;
                    break;
            }
        }
    }
}