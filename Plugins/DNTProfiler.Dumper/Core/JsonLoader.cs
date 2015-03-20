using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;
using DNTProfiler.Dumper.Models;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using DNTProfiler.PluginsBase;
using Newtonsoft.Json;

namespace DNTProfiler.Dumper.Core
{
    public class JsonLoader
    {
        private readonly ProfilerPluginBase _context;
        private readonly MainGuiModel _mainGuiModel;

        public JsonLoader(ProfilerPluginBase context, MainGuiModel mainGuiModel)
        {
            _context = context;
            _mainGuiModel = mainGuiModel;
        }

        public void Start(string filePath)
        {
            _context.NotifyPluginsHost(NotificationType.ShowBusyIndicator, 1);
            var isActive = _mainGuiModel.DumperSettings.IsActive;
            var filesCount = _mainGuiModel.Files.Count;
            _mainGuiModel.DumperSettings.IsActive = false;

            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                var itemsList = JsonHelper.DeserializeFromFile<IList<BaseInfo>>(filePath, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                });
                return itemsList;
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
                    var itemsList = task.Result;
                    if (itemsList == null || !itemsList.Any())
                        return;

                    clearAllData();
                    _context.NotifyPluginsHost(NotificationType.ResetAll, 0);

                    var count = 0;
                    foreach (var item in itemsList)
                    {
                        var baseInfo = item;
                        baseInfo.ReceivedId = IdGenerator.GetId();

                        switch (baseInfo.InfoType)
                        {
                            case InfoType.Command:
                                var command = (Command)baseInfo;
                                command.SetCommandStatistics();
                                _context.ProfilerData.Commands.Add(command);
                                break;
                            case InfoType.CommandConnection:
                                _context.ProfilerData.Connections.Add((CommandConnection)baseInfo);
                                break;
                            case InfoType.CommandResult:
                                _context.ProfilerData.Results.Add((CommandResult)baseInfo);
                                break;
                            case InfoType.CommandTransaction:
                                _context.ProfilerData.Transactions.Add((CommandTransaction)baseInfo);
                                break;
                        }

                        if (count++ % 100 == 0)
                        {
                            DispatcherHelper.DoEvents();
                        }
                    }
                }
                finally
                {
                    _mainGuiModel.DumperSettings.IsActive = isActive;
                    _context.NotifyPluginsHost(NotificationType.HideBusyIndicator, 1);
                    _context.NotifyPluginsHost(NotificationType.Reset, filesCount);
                }
            }, taskScheduler);
        }

        private void clearAllData()
        {
            _context.ProfilerData.ClearAll();
            _mainGuiModel.ClearAll();
            IdGenerator.Reset();
        }
    }
}