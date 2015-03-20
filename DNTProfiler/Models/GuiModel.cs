using System.Collections.Generic;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Models
{
    public class GuiModel : BaseViewModel
    {
        private bool _allowRemoteConnections;
        private bool _isBusy;
        private string _pluginAuthorInfo;
        private IList<Plugin> _plugins;
        private int _pluginsCount;
        private string _pluginSearchText;
        private bool _resetSort;
        private Plugin _selectedPlugin;
        private int _selectedTabIndex;
        private string _serverUri;

        public bool AllowRemoteConnections
        {
            get { return _allowRemoteConnections; }
            set
            {
                if (_allowRemoteConnections == value)
                    return;

                _allowRemoteConnections = value;
                NotifyPropertyChanged(() => AllowRemoteConnections);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged(() => IsBusy);
            }
        }

        public string PluginAuthorInfo
        {
            get { return _pluginAuthorInfo; }
            set
            {
                if (_pluginAuthorInfo == value)
                    return;

                _pluginAuthorInfo = value;
                NotifyPropertyChanged(() => PluginAuthorInfo);
            }
        }

        public IList<Plugin> Plugins
        {
            get { return _plugins; }
            set
            {
                _plugins = value;
                NotifyPropertyChanged(() => Plugins);
                if (value != null) PluginsCount = value.Count;
            }
        }

        public int PluginsCount
        {
            get { return _pluginsCount; }
            set
            {
                _pluginsCount = value;
                NotifyPropertyChanged(() => PluginsCount);
            }
        }

        public string PluginSearchText
        {
            get { return _pluginSearchText; }
            set
            {
                if (_pluginSearchText == value)
                    return;

                _pluginSearchText = value;
                NotifyPropertyChanged(() => PluginSearchText);
            }
        }

        public bool ResetSort
        {
            get { return _resetSort; }
            set
            {
                if (_resetSort == value)
                    return;

                _resetSort = value;
                NotifyPropertyChanged(() => ResetSort);
            }
        }

        public Plugin SelectedPlugin
        {
            get { return _selectedPlugin; }
            set
            {
                if (value == null)
                    return;
                if (_selectedPlugin != null && value.Id == _selectedPlugin.Id)
                    return;

                _selectedPlugin = value;
                NotifyPropertyChanged(() => SelectedPlugin);
            }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (_selectedTabIndex == value)
                    return;

                _selectedTabIndex = value;
                NotifyPropertyChanged(() => SelectedTabIndex);
            }
        }

        public string ServerUri
        {
            get { return _serverUri; }
            set
            {
                if (_serverUri == value)
                    return;

                _serverUri = value;
                NotifyPropertyChanged(() => ServerUri);
            }
        }
    }
}