using System.Collections.Generic;
using System.Collections.Specialized;
using DNTProfiler.Common.ClipboardUtils;
using DNTProfiler.Common.Controls.DialogManagement.Contracts;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using DNTProfiler.Infrastructure.StackTraces;
using DNTProfiler.Infrastructure.Views;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.Infrastructure.ViewModels
{
    public class MainViewModelBase : BaseViewModel
    {
        protected readonly ProfilerPluginBase PluginContext;

        public MainViewModelBase(ProfilerPluginBase pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            PluginContext = pluginContext;
            GuiModelData = new GuiModelBase();
            DoOpenFile = new DelegateCommand<CallingMethodInfo>(OpenFile, info => true);
            DoCopySelectedLine = new DelegateCommand<object>(CopySelectedLine, info => true);
            DoCopyAllLines = new DelegateCommand<object>(CopyAllLines, info => true);
            DoOpenPopupToolTip = new DelegateCommand<object>(OpenPopupToolTip, info => true);
            DoOpenCommandToolTip = new DelegateCommand<Command>(OpenCommandToolTip, info => true);
            PluginContext.ProfilerData.ApplicationIdentities.CollectionChanged += ApplicationIdentities_CollectionChanged;
        }

        public DelegateCommand<object> DoCopyAllLines { set; get; }

        public DelegateCommand<object> DoCopySelectedLine { set; get; }

        public DelegateCommand<Command> DoOpenCommandToolTip { set; get; }

        public DelegateCommand<CallingMethodInfo> DoOpenFile { set; get; }
        public DelegateCommand<object> DoOpenPopupToolTip { set; get; }

        public GuiModelBase GuiModelData { set; get; }

        protected virtual void ApplicationIdentities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (AppIdentity item in e.NewItems)
                    {
                        GuiModelData.ApplicationIdentities.Add(new AppIdentity(item));
                    }
                    break;
            }
        }

        protected virtual void CopyAllLines(object data)
        {
            if (data == null)
                return;

            var rows = (IEnumerable<object>)data;
            rows.CopyToClipboard();
        }

        protected virtual void CopySelectedLine(object data)
        {
            if (data == null)
                return;

            data.CopyToClipboard();
        }

        protected virtual void OpenCommandToolTip(Command command)
        {
            if (command == null)
                return;

            command.Sql = command.Sql.FormatTSql(command.SqlHash);

            var dlg = PluginContext.CustomDialogsService.CreateCustomContentDialog(
                            new CommandTooltip { Command = command }, "Details", DialogMode.Ok);
            dlg.Show();
        }

        protected virtual void OpenFile(CallingMethodInfo data)
        {
            if (data == null)
                return;

            new OpenStackTraceFile
            {
                Column = data.CallingCol,
                Line = data.CallingLine,
                FullFilename = data.CallingFileFullName
            }.ShowToUser();
        }

        protected virtual void OpenPopupToolTip(object data)
        {
            if (data == null)
                return;

            data = data.TryGetMethodBody();
            if (!(data is string))
            {
                data = data.ToFormattedJson();
            }

            var dlg = PluginContext.CustomDialogsService.CreateCustomContentDialog(
                            new PopupToolTip { Text = data.ToString() }, "Details", DialogMode.Ok);
            dlg.Show();
        }

        protected void ResetAll()
        {
            PluginContext.ProfilerData.ClearAll();
            GuiModelData.ClearAll();
        }
    }
}