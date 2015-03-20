using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByConnections.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageCommandConnections(CommandConnection item)
        {
            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedConnections.Add(item);
            }

            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item);
        }
    }
}