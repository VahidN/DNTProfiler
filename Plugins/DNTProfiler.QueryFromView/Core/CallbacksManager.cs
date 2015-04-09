using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.QueryFromView.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private static readonly string[] _viewExtensions =
        {
            ".aspx",
            ".ascx",
            ".master",
            ".cshtml",
            ".vbhtml"
        };

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

            if (!hasQueryFromView(stackTrace))
            {
                return;
            }

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

        private static bool hasQueryFromView(CallingMethodStackTrace item)
        {
            if (item == null || item.CallingMethodInfoList == null)
                return false;

            foreach (var info in item.CallingMethodInfoList)
            {
                if (string.IsNullOrWhiteSpace(info.CallingFile))
                    continue;

                var ext = Path.GetExtension(info.CallingFile);
                if (string.IsNullOrWhiteSpace(ext))
                    continue;

                ext = ext.ToLowerInvariant();

                if (_viewExtensions.Contains(ext))
                {
                    return true;
                }
            }

            return false;
        }
    }
}