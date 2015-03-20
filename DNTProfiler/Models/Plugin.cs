using System.Reflection;
using System.Windows.Controls;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.Models
{
    public class Plugin : BaseViewModel
    {
        private UserControl _content;
        private int _notificationsCount;

        public PluginCategory Category
        {
            get { return ProfilerPlugin.Category; }
        }

        public UserControl Content
        {
            get
            {
                if (_content != null)
                    return _content;

                DispatcherHelper.DispatchAction(() =>
                {
                    _content = ProfilerPlugin.GetPluginUI();
                });
                return _content;
            }
        }

        public string Description
        {
            get { return ProfilerPlugin.PluginMetadata.PluginDescription; }
        }

        public string Header
        {
            get { return ProfilerPlugin.PluginMetadata.PluginName; }
        }

        public int Id { set; get; }

        public int NotificationsCount
        {
            get { return _notificationsCount; }
            set
            {
                _notificationsCount = value;
                NotifyPropertyChanged(() => NotificationsCount);
            }
        }

        public Assembly PluginAssembly { set; get; }

        public ProfilerPluginBase ProfilerPlugin { set; get; }

        public override string ToString()
        {
            return Header;
        }
    }
}