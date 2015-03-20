using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.NonDisposedContexts.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<CommandConnection> _localCommandConnection = new ObservableCollection<CommandConnection>();

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void AddNewOpenedConnections(CommandConnection item)
        {
            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedConnections.Add(item);
            }

            _localCommandConnection.Add(item);
            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item);
        }

        public void RemoveDisposedConnection(CommandConnection item)
        {
            if (item.Type != CommandConnectionType.Disposed)
                return;

            var connections = _localCommandConnection
                               .Where(x => x.ConnectionId == item.ConnectionId &&
                                           x.ApplicationIdentity.Equals(item.ApplicationIdentity))
                               .ToList();
            foreach (var commandConnection in connections)
            {
                _localCommandConnection.Remove(commandConnection);
                PluginContext.NotifyPluginsHost(NotificationType.Update, -1);
                UpdateAppIdentityNotificationsCount(item, -1);
            }

            connections = GuiModelData.RelatedConnections
                               .Where(x => x.ConnectionId == item.ConnectionId &&
                                           x.ApplicationIdentity.Equals(item.ApplicationIdentity))
                               .ToList();
            foreach (var commandConnection in connections)
            {
                GuiModelData.RelatedConnections.Remove(commandConnection);
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