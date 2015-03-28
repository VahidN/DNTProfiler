using System;
using System.Collections.ObjectModel;
using System.Linq;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Core;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.StaticResourcesQueries.Core
{
    public class CallbacksManager : CallbacksManagerBase
    {
        private readonly ObservableCollection<TrafficUrl> _localTrafficUrls = new ObservableCollection<TrafficUrl>();

        public CallbacksManager(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
            : base(pluginContext, guiModelData)
        {
        }

        public void ManageTrafficUrls(TrafficUrl item)
        {
            if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
            {
                GuiModelData.RelatedTrafficUrls.Add(item);
            }

            _localTrafficUrls.Add(item);
            PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
            UpdateAppIdentityNotificationsCount(item.ApplicationIdentity);
        }

        public void Reset()
        {
            _localTrafficUrls.Clear();
        }

        public void ShowSelectedApplicationIdentityTrafficUrls()
        {
            GuiModelData.RelatedTrafficUrls.Clear();
            GuiModelData.RelatedTrafficUrls =
                new ObservableCollection<TrafficUrl>(
                    _localTrafficUrls.Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));
        }
    }
}