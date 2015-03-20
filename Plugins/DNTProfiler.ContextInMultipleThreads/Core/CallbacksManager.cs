using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ContextInMultipleThreads.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void CheckObjectContextsInMultipleThreads(Command item)
        {
            var context = GetContext(item);
            if (context == null)
                return;

            if (context.ManagedThreadId == item.ManagedThreadId)
                return;

            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedCommands.Add(item);
            }
            GuiModelData.LocalCommands.Add(item);
            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item);
        }
    }
}