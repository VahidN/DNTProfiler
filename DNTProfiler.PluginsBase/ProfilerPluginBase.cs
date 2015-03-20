using System;
using System.Windows.Controls;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.PluginsBase
{
    public abstract class ProfilerPluginBase
    {
        public PluginCategory Category { set; get; }

        public ICommonDialogsService CommonDialogsService { set; get; }

        public IDialogManager CustomDialogsService { set; get; }

        public Func<string> GetResults { set; get; }

        public Action MainWindowIsLoaded { set; get; }

        public Action<NotificationType, int> NotifyPluginsHost { set; get; }

        public PluginAuthor PluginAuthor { get; set; }

        public PluginMetadata PluginMetadata { get; set; }

        public ProfilerData ProfilerData { get; set; }

        public Action Reset { set; get; }

        public abstract UserControl GetPluginUI();
    }
}