using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.MultipleContextPerRequest.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<TrafficWebRequest> _localTrafficWebRequest = new ObservableCollection<TrafficWebRequest>();

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageTrafficWebRequest(TrafficWebRequest item)
        {
            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedTrafficWebRequests.Add(item);
            }
            _localTrafficWebRequest.Add(item);

            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item.ApplicationIdentity);
        }

        public void Reset()
        {
            _localTrafficWebRequest.Clear();
        }

        public void ShowRelatedCommandsOfSelectedTrafficWebRequest()
        {
            if (GuiModelData.SelectedTrafficWebRequest == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedCommands.Clear();

            var commands = PluginContext.ProfilerData.Commands
                            .Where(x => x.HttpInfo.HttpContextCurrentId == GuiModelData.SelectedTrafficWebRequest.HttpContextCurrentId &&
                                        x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                            .OrderBy(x => x.AtDateTime)
                            .ToList();

            foreach (var item in commands)
            {
                GuiModelData.RelatedCommands.Add(item);
            }

            ActivateRelatedStackTraces();
        }

        public void ShowSelectedApplicationIdentityRelatedTrafficWebRequests()
        {
            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedTrafficWebRequests.Clear();
            GuiModelData.RelatedTrafficWebRequests =
                new ObservableCollection<TrafficWebRequest>(
                    _localTrafficWebRequest.Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));

        }
    }
}