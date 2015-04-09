using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByMethods.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<CallingMethodStackTrace> _localCallingMethodStackTraces = new ObservableCollection<CallingMethodStackTrace>();

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageStackTraces(Command command)
        {
            var stackTrace = GetStackTrace(command);
            if (stackTrace == null)
                return;

            stackTrace.ApplicationIdentity = command.ApplicationIdentity;
            if (stackTrace.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedStackTraces.Add(stackTrace);
            }

            _localCallingMethodStackTraces.Add(stackTrace);
            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(stackTrace.ApplicationIdentity);
        }

        public void Reset()
        {
            _localCallingMethodStackTraces.Clear();
        }

        public void ShowSelectedApplicationIdentityStackTraces()
        {
            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedStackTraces =
                new ObservableCollection<CallingMethodStackTrace>(
                    _localCallingMethodStackTraces.Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));
        }
    }
}