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

        public void ManageStackTraces(CallingMethodStackTrace item)
        {
            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedStackTraces.Add(item);
            }

            _localCallingMethodStackTraces.Add(item);
            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item.ApplicationIdentity);
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