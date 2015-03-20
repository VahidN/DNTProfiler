using System.Text;
using DNTProfiler.ApplicationAnnouncements.Core;
using DNTProfiler.ApplicationAnnouncements.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ApplicationAnnouncements.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            ThisGuiModelData = new MainGuiModel();
            setActions();
        }

        public MainGuiModel ThisGuiModelData { set; get; }

        private void getInfo()
        {
            new ProjectReleasesLoader(PluginContext, ThisGuiModelData).GetReleaseInfo(ProjectConfig.ProjectReleasesUrl);
        }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
            };

            PluginContext.MainWindowIsLoaded = () =>
            {
                getInfo();
            };

            PluginContext.GetResults = () =>
            {
                var stringBuilder = new StringBuilder();
                foreach (var releaseInfo in ThisGuiModelData.ReleaseInfo)
                {
                    stringBuilder.AppendLine(releaseInfo.AssetName);
                }
                return stringBuilder.ToString();
            };
        }
    }
}