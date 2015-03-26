using System;
using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.DuplicateCommandsPerMethod.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<CallingMethodStackTrace> _localCallingMethodStackTraces = new ObservableCollection<CallingMethodStackTrace>();

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageStackTraces(Command item)
        {
            if (!HasThisMethodDuplicateQueriesWithSameHash(item))
                return;

            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item.ApplicationIdentity);

            if (shouldIgnoreAddingThisMethod(item))
                return;

            var stackTrace = GetStackTrace(item);
            if (stackTrace == null)
                return;

            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedStackTraces.Add(stackTrace);
            }

            _localCallingMethodStackTraces.Add(stackTrace);
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

        private bool shouldIgnoreAddingThisMethod(BaseInfo item)
        {
            return _localCallingMethodStackTraces.Any(
                x => x.StackTraceHash.Equals(item.StackTrace.StackTraceHash, StringComparison.OrdinalIgnoreCase) &&
                     x.ApplicationIdentity.Equals(item.ApplicationIdentity));
        }
    }
}