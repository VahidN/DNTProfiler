using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DNTProfiler.ApplicationAnnouncements.Models;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.WebToolkit;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationAnnouncements.Core
{
    public class ProjectReleasesLoader
    {
        private readonly ProfilerPluginBase _context;
        private readonly MainGuiModel _mainGuiModel;

        public ProjectReleasesLoader(ProfilerPluginBase context, MainGuiModel mainGuiModel)
        {
            _context = context;
            _mainGuiModel = mainGuiModel;
        }

        public void GetReleaseInfo(string url)
        {
            _context.NotifyPluginsHost(NotificationType.ShowBusyIndicator, 1);

            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                if (!NetworkStatus.IsConnectedToInternet())
                    return null;

                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("user-agent", "DNTProfiler");
                    var jsonData = webClient.DownloadString(url);
                    var gitHubProjectReleases = JsonHelper.DeserializeObject<GitHubProjectRelease[]>(jsonData);

                    var releases = new List<ReleaseInfo>();
                    foreach (var release in gitHubProjectReleases)
                    {
                        foreach (var asset in release.Assets)
                        {
                            releases.Add(new ReleaseInfo
                            {
                                ReleaseHtmlUrl = release.HtmlUrl,
                                ReleaseTagName = release.TagName,
                                AssetCreatedAt = asset.CreatedAt,
                                AssetName = asset.Name,
                                AssetSize = asset.Size,
                                AssetDownloadCount = asset.DownloadCount
                            });
                        }
                    }

                    return releases.OrderByDescending(releaseInfo => releaseInfo.AssetCreatedAt).ToList();
                }
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                    {
                        task.Exception.Flatten().Handle(ex =>
                        {
                            new ExceptionLogger().LogExceptionToFile(ex, AppMessenger.LogFile);
                            AppMessenger.Messenger.NotifyColleagues("ShowException", ex);
                            return true;
                        });
                    }

                    _context.NotifyPluginsHost(NotificationType.HideBusyIndicator, 1);
                    return;
                }

                try
                {
                    var releaseInfo = task.Result;
                    if (releaseInfo == null || !releaseInfo.Any())
                        return;

                    _context.NotifyPluginsHost(NotificationType.ResetAll, 0);
                    _mainGuiModel.ReleaseInfo = releaseInfo;
                    _context.NotifyPluginsHost(NotificationType.Reset, releaseInfo.Count);
                }
                finally
                {
                    _context.NotifyPluginsHost(NotificationType.HideBusyIndicator, 1);
                }
            }, taskScheduler);
        }
    }
}