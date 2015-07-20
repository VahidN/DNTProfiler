using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.LongConnections.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<CommandConnection> _localCommandConnection = new ObservableCollection<CommandConnection>();
        const int OneSecond = 1000;

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageDisposedConnection(CommandConnection disposedItem)
        {
            if (disposedItem.Type != CommandConnectionType.Disposed)
                return;

            var opendConnection = PluginContext.ProfilerData.Connections
                                           .FirstOrDefault(x => x.ConnectionId == disposedItem.ConnectionId &&
                                                       x.Type == CommandConnectionType.Opened &&
                                                       x.ApplicationIdentity.Equals(disposedItem.ApplicationIdentity));
            if (opendConnection == null)
            {
                return;
            }

            if (opendConnection.Duration >= OneSecond)
            {
                if (disposedItem.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                {
                    GuiModelData.RelatedConnections.Add(opendConnection);
                }

                _localCommandConnection.Add(opendConnection);
                PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                UpdateAppIdentityNotificationsCount(disposedItem);
            }
        }

        public void Reset()
        {
            _localCommandConnection.Clear();
        }

        public void ShowSelectedApplicationIdentityLocalConnections()
        {
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedConnections =
                new ObservableCollection<CommandConnection>(
                    _localCommandConnection.Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));
        }
    }
}