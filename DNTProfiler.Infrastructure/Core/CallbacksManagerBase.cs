using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.Models;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.Infrastructure.Core
{
    public class CallbacksManagerBase
    {
        protected readonly GuiModelBase GuiModelData;
        protected readonly ProfilerPluginBase PluginContext;

        public CallbacksManagerBase(ProfilerPluginBase pluginContext, GuiModelBase guiModelData)
        {
            PluginContext = pluginContext;
            GuiModelData = guiModelData;
        }

        public void ActivateRelatedStackTraces()
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
                return;

            if (GuiModelData.RelatedCommands.Any())
            {
                GuiModelData.SelectedExecutedCommand =
                    GuiModelData.RelatedCommands.FirstOrDefault(
                            x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity));
            }
        }

        public void AddStackTrace(BaseInfo item)
        {
            var stackTrace = GetStackTrace(item);
            if (stackTrace != null)
                return;

            item.StackTrace.ApplicationIdentity = item.ApplicationIdentity;
            PluginContext.ProfilerData.StackTraces.Add(item.StackTrace);
        }

        public void AddTrafficUrl(BaseInfo item)
        {
            if (string.IsNullOrWhiteSpace(item.HttpInfo.Url))
                return;

            var trafficUrl = GetTrafficUrl(item);
            if (trafficUrl == null)
            {
                PluginContext.ProfilerData.TrafficUrls.Add(new TrafficUrl
                {
                    Url = item.HttpInfo.Url,
                    UrlHash = item.HttpInfo.UrlHash,
                    ApplicationIdentity = item.ApplicationIdentity,
                    IsStaticFile = item.HttpInfo.IsStaticFile
                });
            }
        }

        public void CreateOrUpdateContext(CommandConnection item)
        {
            var isNewContext = false;

            var context = GetContext(item);
            if (context != null)
            {
                context.ContextStatistics.NumberOfConnections++;
            }
            else
            {
                isNewContext = true;
                context = new Context
                {
                    ObjectContextId = item.ObjectContextId,
                    Url = item.HttpInfo.Url,
                    AtDateTime = item.AtDateTime,
                    ObjectContextName = item.ObjectContextName,
                    ContextStatistics = { NumberOfConnections = 1 },
                    ManagedThreadId = item.ManagedThreadId,
                    HttpContextCurrentId = item.HttpInfo.HttpContextCurrentId,
                    ApplicationIdentity = item.ApplicationIdentity
                };
                PluginContext.ProfilerData.Contexts.Add(context);

                if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                {
                    GuiModelData.Contexts.Add(context);
                }

                PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                UpdateAppIdentityNotificationsCount(context.ApplicationIdentity);
            }

            UpdateMultipleContextPerRequestInfo(context);
            UpdateNumberOfConnections(GetStackTrace(item));

            var trafficUrl = GetTrafficUrl(item);
            UpdateNumberOfConnections(trafficUrl);
            if (trafficUrl != null && isNewContext)
            {
                trafficUrl.NumberOfContexts++;
            }
        }

        public void CreateOrUpdateContext(Command item)
        {
            var isNewContext = false;

            var context = GetContext(item);
            if (context != null)
            {
                UpdateNumberOfQueries(context);
                UpdateNumberOfDuplicateQueries(context, item);
            }
            else
            {
                isNewContext = true;

                context = new Context
                {
                    ObjectContextId = item.ObjectContextId,
                    Url = item.HttpInfo.Url,
                    AtDateTime = item.AtDateTime,
                    ContextStatistics = { NumberOfQueries = 1 },
                    ObjectContextName = item.ObjectContextName,
                    ManagedThreadId = item.ManagedThreadId,
                    HttpContextCurrentId = item.HttpInfo.HttpContextCurrentId,
                    ApplicationIdentity = item.ApplicationIdentity
                };
                PluginContext.ProfilerData.Contexts.Add(context);

                if (item.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                {
                    GuiModelData.Contexts.Add(context);
                }

                PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                UpdateAppIdentityNotificationsCount(context.ApplicationIdentity);
            }

            UpdateMultipleContextPerRequestInfo(context);
            UpdateNumberOfQueries(GetStackTrace(item));

            var trafficUrl = GetTrafficUrl(item);
            UpdateNumberOfQueries(trafficUrl);
            if (trafficUrl != null && isNewContext)
            {
                trafficUrl.NumberOfContexts++;
            }
        }

        public void FinishRelatedCommand(CommandResult item)
        {
            var command = PluginContext.ProfilerData.Commands
                                        .FirstOrDefault(x => x.CommandId == item.CommandId &&
                                                             x.ApplicationIdentity.Equals(item.ApplicationIdentity));

            if (command != null)
            {
                command.ResultException = item.Exception;
                command.IsCanceled = item.IsCanceled;
                command.ElapsedMilliseconds = item.ElapsedMilliseconds;
                command.FieldsCount = item.FieldsCount;
                command.CommandMemoryUsage = item.AppDomainSnapshot.TotalAllocatedMemorySize - command.AppDomainSnapshot.TotalAllocatedMemorySize;
                command.CommandCpuUsage = item.AppDomainSnapshot.TotalProcessorTime - command.AppDomainSnapshot.TotalProcessorTime;
            }

            UpdateTotalQueryExecutionTime(item, GetContext(item));
            UpdateTotalQueryExecutionTime(item, GetStackTrace(item));
            UpdateTotalQueryExecutionTime(item, GetTrafficUrl(item));
        }

        public void RunAnalysisVisitorOnCommand(SqlFragmentVisitorBase visitor, Command command)
        {
            RunAnalysisVisitorOnCommand(
                () => RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(command.Sql, command.SqlHash, visitor), command);
        }

        public void RunAnalysisVisitorOnCommand(Func<bool> jobFunc, Command command)
        {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                return jobFunc();
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
                    return;
                }

                if (!task.Result) return;

                if (Equals(GuiModelData.SelectedApplicationIdentity, command.ApplicationIdentity))
                {
                    GuiModelData.RelatedCommands.Add(command);
                }

                GuiModelData.LocalCommands.Add(command);

                PluginContext.NotifyPluginsHost(NotificationType.Update, 1);
                UpdateAppIdentityNotificationsCount(command);
            }, taskScheduler);
        }

        public void SetConnectionAndContexAsDisposed(CommandConnection item)
        {
            if (item.Type != CommandConnectionType.Disposed)
                return;

            var connections = PluginContext.ProfilerData.Connections
                                           .Where(x => x.ConnectionId == item.ConnectionId &&
                                                       x.ApplicationIdentity.Equals(item.ApplicationIdentity))
                                           .ToList();
            foreach (var commandConnection in connections)
            {
                commandConnection.DisposedAt = item.AtDateTime;
            }

            var contextIds = connections.Select(x => x.ObjectContextId).ToList();
            var contexts = PluginContext.ProfilerData.Contexts.Where(x => contextIds.Contains(x.ObjectContextId));
            foreach (var context in contexts.Where(x => x.DisposedAt == null))
            {
                context.DisposedAt = item.AtDateTime;
            }
        }

        public void ShowAllCommandsWithSameConnectionId(int connectionId)
        {
            var connection =
                PluginContext.ProfilerData.Connections.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (connection == null)
                return;

            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            var commands = PluginContext.ProfilerData.Commands
                .Where(command => command.ConnectionId == connection.ConnectionId &&
                                  command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .OrderBy(command => command.AtDateTime)
                .ToList();

            GuiModelData.RelatedCommands = new ObservableCollection<Command>(commands);

            ActivateRelatedStackTraces();
        }

        public void ShowSelectedApplicationIdentityConnections()
        {
            GuiModelData.Contexts.Clear();
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedConnections =
                new ObservableCollection<CommandConnection>(
                    PluginContext.ProfilerData.Connections
                        .Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity) &&
                               x.Type == CommandConnectionType.Opened));
        }

        public void ShowSelectedApplicationIdentityContexts()
        {
            GuiModelData.Contexts.Clear();
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.Contexts =
                new ObservableCollection<Context>(
                    PluginContext.ProfilerData.Contexts.Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));
        }

        public void ShowSelectedApplicationIdentityLocalCommands()
        {
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedCommands =
                new ObservableCollection<Command>(
                    GuiModelData.LocalCommands.Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity)));
        }

        public void ShowSelectedApplicationIdentityTransactions()
        {
            GuiModelData.Contexts.Clear();
            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedTransactions.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedTransactions =
                new ObservableCollection<CommandTransaction>(
                    PluginContext.ProfilerData.Transactions
                        .Where(x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity) &&
                               x.TransactionType == CommandTransactionType.Began));
        }

        public void ShowSelectedCommand(int commandId)
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            var commands = PluginContext.ProfilerData.Commands
                .Where(command => command.CommandId == commandId &&
                                  command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .OrderBy(command => command.AtDateTime)
                .ToList();

            GuiModelData.RelatedCommands = new ObservableCollection<Command>(commands);

            ActivateRelatedStackTraces();
        }

        public void ShowSelectedCommandRelatedStackTraces()
        {
            if (GuiModelData.SelectedExecutedCommand == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedStackTraces.Clear();

            var callingMethods = PluginContext.ProfilerData.StackTraces
                .Where(x =>
                    x.StackTraceHash.Equals(GuiModelData.SelectedExecutedCommand.StackTrace.StackTraceHash, StringComparison.OrdinalIgnoreCase) &&
                    x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .ToList();

            foreach (var stackTrace in callingMethods)
            {
                GuiModelData.RelatedStackTraces.Add(stackTrace);
            }
        }

        public void ShowSelectedConnectionRelatedCommands()
        {
            var connection = GuiModelData.SelectedConnection;
            ShowSelectedConnectionRelatedCommands(connection);
        }

        public void ShowSelectedContextRelatedCommands()
        {
            if (GuiModelData.SelectedContext == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedCommands.Clear();

            var commands = PluginContext.ProfilerData.Commands
                            .Where(x => x.ObjectContextId == GuiModelData.SelectedContext.ObjectContextId &&
                                        x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                            .OrderBy(x => x.AtDateTime)
                            .ToList();

            foreach (var item in commands)
            {
                GuiModelData.RelatedCommands.Add(item);
            }

            ActivateRelatedStackTraces();
        }

        public void ShowSelectedContextRelatedConnections()
        {
            if (GuiModelData.SelectedContext == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedConnections.Clear();
            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            var connections = PluginContext.ProfilerData.Connections
                .Where(x => x.ObjectContextId == GuiModelData.SelectedContext.ObjectContextId &&
                            x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity) &&
                            x.Type == CommandConnectionType.Opened)
                .OrderBy(x => x.AtDateTime)
                .ToList();

            foreach (var item in connections)
            {
                GuiModelData.RelatedConnections.Add(item);
            }

            ActivateRelatedCommands();
        }

        public void ShowSelectedStackTraceRelatedCommands()
        {
            if (GuiModelData.SelectedStackTrace == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();

            var relatedCommands = PluginContext.ProfilerData.Commands
                .Where(
                    x =>
                        x.StackTrace.StackTraceHash.Equals(GuiModelData.SelectedStackTrace.StackTraceHash, StringComparison.OrdinalIgnoreCase) &&
                        x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .ToList();

            foreach (var command in relatedCommands)
            {
                GuiModelData.RelatedCommands.Add(command);
            }
        }

        public void ShowSelectedStackTraceRelatedDuplicateCommands()
        {
            if (GuiModelData.SelectedStackTrace == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();

            var relatedCommands = PluginContext.ProfilerData.Commands
                .Where(
                    x =>
                        x.StackTrace.StackTraceHash.Equals(GuiModelData.SelectedStackTrace.StackTraceHash, StringComparison.OrdinalIgnoreCase) &&
                        x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                .ToList();

            foreach (var item in relatedCommands.Where(item => relatedCommands.Any(
                            command => command.CommandId != item.CommandId &&
                           (command.SqlHash.Equals(item.SqlHash, StringComparison.OrdinalIgnoreCase) ||
                            command.NormalizedSqlHash.Equals(item.NormalizedSqlHash, StringComparison.OrdinalIgnoreCase)))))
            {
                GuiModelData.RelatedCommands.Add(item);
            }
        }

        public void ShowSelectedTrafficUrlRelatedCommands()
        {
            if (GuiModelData.SelectedTrafficUrl == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedStackTraces.Clear();
            GuiModelData.RelatedCommands.Clear();

            var commands = PluginContext.ProfilerData.Commands
                            .Where(x =>
                                x.HttpInfo.UrlHash.Equals(GuiModelData.SelectedTrafficUrl.UrlHash, StringComparison.OrdinalIgnoreCase) &&
                                x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity))
                            .OrderBy(x => x.AtDateTime)
                            .ToList();

            foreach (var item in commands)
            {
                GuiModelData.RelatedCommands.Add(item);
            }

            ActivateRelatedStackTraces();
        }

        public void ShowSelectedTransactionRelatedCommands()
        {
            var tx = GuiModelData.SelectedTransaction;
            if (tx == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            if (!tx.CommandsIds.Any())
                return;

            var commands = PluginContext.ProfilerData.Commands
                .Where(command => command.ConnectionId == tx.ConnectionId &&
                                  command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity) &&
                                  command.ObjectContextId == tx.ObjectContextId &&
                                  command.TransactionId == tx.TransactionId &&
                                  command.AtDateTime >= tx.AtDateTime &&
                                  tx.CommandsIds.Contains(command.CommandId ?? 0))
                .OrderBy(command => command.AtDateTime)
                .ToList();

            foreach (var item in commands)
            {
                GuiModelData.RelatedCommands.Add(item);
            }

            ActivateRelatedStackTraces();
        }

        public void UpdateAppIdentityNotificationsCount(BaseInfo info, int count = 1)
        {
            UpdateAppIdentityNotificationsCount(info.ApplicationIdentity, count);
        }

        public void UpdateAppIdentityNotificationsCount(AppIdentity itemIdentity, int count = 1)
        {
            if (itemIdentity == null)
                return;

            var identity = GuiModelData.ApplicationIdentities
                     .FirstOrDefault(x => x.Equals(itemIdentity));

            if (identity == null)
            {
                UpdateApplicationIdentities(itemIdentity);
                identity = GuiModelData.ApplicationIdentities
                    .First(x => x.Equals(itemIdentity));
            }

            identity.NotificationsCount += count;
        }

        public void UpdateApplicationIdentities(BaseInfo info)
        {
            UpdateApplicationIdentities(info.ApplicationIdentity);
        }

        public void UpdateApplicationIdentities(AppIdentity info)
        {
            if (!PluginContext.ProfilerData.ApplicationIdentities.Contains(info))
            {
                PluginContext.ProfilerData.ApplicationIdentities.Add(info);
            }
            ActivateFirstAppIdentity();
        }

        public void UpdateCommandsStatistics(Command item)
        {
            UpdateCommandStatistics(item, GetContext(item));
            UpdateCommandStatistics(item, GetStackTrace(item));
            UpdateCommandStatistics(item, GetTrafficUrl(item));
        }

        public void UpdateConnectionCommandsCount(Command command)
        {
            if (command == null)
                return;

            var commandConnection = PluginContext.ProfilerData.Connections
                .FirstOrDefault(connection => connection.ConnectionId == command.ConnectionId &&
                                     connection.Type == CommandConnectionType.Opened &&
                                     connection.ClosedAt == null &&
                                     connection.ObjectContextId == command.ObjectContextId &&
                                     connection.TransactionId == command.TransactionId &&
                                     connection.ApplicationIdentity.Equals(command.ApplicationIdentity)
                                     );
            if (commandConnection == null)
                return;

            commandConnection.CommandsCount++;

            if (command.CommandId != null)
                commandConnection.CommandsIds.Add(item: command.CommandId.Value);
        }

        public void UpdateConnectionsClosedTime(CommandConnection item)
        {
            if (item.ClosedAt != null || item.Type == CommandConnectionType.Opened)
                return;

            item.ClosedAt = item.AtDateTime;

            var connection =
                PluginContext.ProfilerData.Connections.FirstOrDefault(
                    x => x.ConnectionId == item.ConnectionId &&
                         x.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                         x.ClosedAt == null);
            if (connection == null)
                return;

            connection.ClosedAt = item.ClosedAt;

            UpdateTotalConnectionOpenTime(GetContext(item), connection);
            UpdateTotalConnectionOpenTime(GetStackTrace(item), connection);
            UpdateTotalConnectionOpenTime(GetTrafficUrl(item), connection);
        }

        public void UpdateNewConnectionsTransactionIds(CommandTransaction item)
        {
            var lastNewConnection = PluginContext.ProfilerData.Connections
                .LastOrDefault(
                        x =>
                            x.ConnectionId == item.ConnectionId &&
                            x.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                            x.TransactionId == null);

            if (lastNewConnection != null)
            {
                lastNewConnection.TransactionId = item.TransactionId;
            }
        }

        public void UpdateNumberOfCommittedTransactions(CommandTransaction item)
        {
            UpdateNumberOfCommittedTransactions(GetContext(item));
            UpdateNumberOfCommittedTransactions(GetStackTrace(item));
            UpdateNumberOfCommittedTransactions(GetTrafficUrl(item));
        }

        public void UpdateNumberOfExceptions(CommandResult item)
        {
            if (string.IsNullOrWhiteSpace(item.Exception))
                return;

            UpdateNumberOfExceptions(GetContext(item));
            UpdateNumberOfExceptions(GetStackTrace(item));
            UpdateNumberOfExceptions(GetTrafficUrl(item));
        }

        public void UpdateNumberOfRolledBackTransactions(CommandTransaction item)
        {
            UpdateNumberOfRolledBackTransactions(GetContext(item));
            UpdateNumberOfRolledBackTransactions(GetStackTrace(item));
            UpdateNumberOfRolledBackTransactions(GetTrafficUrl(item));
        }

        public void UpdateNumberOfTransactions(CommandTransaction item)
        {
            UpdateNumberOfTransactions(GetContext(item));
            UpdateNumberOfTransactions(GetStackTrace(item));
            UpdateNumberOfTransactions(GetTrafficUrl(item));
        }

        public void UpdateRelatedCommandsRowCount(CommandResult item)
        {
            var command = PluginContext.ProfilerData.Commands
                            .FirstOrDefault(x => x.CommandId == item.CommandId &&
                                                 x.ApplicationIdentity.Equals(item.ApplicationIdentity));
            if (command == null)
                return;

            command.RowsReturned = item.RowsReturned;

            UpdateTotalNumberOfRowsReturned(item, GetContext(item));
            UpdateTotalNumberOfRowsReturned(item, GetStackTrace(item));
            UpdateTotalNumberOfRowsReturned(item, GetTrafficUrl(item));
        }

        public void UpdateThreadsList(BaseInfo item)
        {
            var context = GetContext(item);
            if (context == null)
                return;

            if (context.ContextStatistics.UsedInThreadIds.Contains(item.ManagedThreadId))
                return;

            context.ContextStatistics.UsedInThreadIds.Add(item.ManagedThreadId);
            context.ContextStatistics.TotalNumberOfThreads = context.ContextStatistics.UsedInThreadIds.Count;
        }

        public void UpdateTransactionsCommandsCount(Command command)
        {
            if (command == null)
                return;

            var commandTransaction = PluginContext.ProfilerData.Transactions
                .FirstOrDefault(transaction => transaction.ConnectionId == command.ConnectionId &&
                                     transaction.TransactionType == CommandTransactionType.Began &&
                                     transaction.ClosedAt == null &&
                                     transaction.ObjectContextId == command.ObjectContextId &&
                                     transaction.TransactionId == command.TransactionId &&
                                     transaction.ApplicationIdentity.Equals(command.ApplicationIdentity)
                                    );
            if (commandTransaction == null)
                return;

            commandTransaction.CommandsCount++;

            if (command.CommandId != null)
                commandTransaction.CommandsIds.Add(item: command.CommandId.Value);
        }

        protected static void UpdateCommandStatistics(Command item, StatisticsBase info)
        {
            if (info == null) return;
            info.CommandStatistics.DeletesCount += item.CommandStatistics.DeletesCount;
            info.CommandStatistics.InsertsCount += item.CommandStatistics.InsertsCount;
            info.CommandStatistics.JoinsCount += item.CommandStatistics.JoinsCount;
            info.CommandStatistics.LikesCount += item.CommandStatistics.LikesCount;
            info.CommandStatistics.SelectsCount += item.CommandStatistics.SelectsCount;
            info.CommandStatistics.UpdatesCount += item.CommandStatistics.UpdatesCount;
        }

        protected static void UpdateNumberOfCommittedTransactions(StatisticsBase info)
        {
            if (info == null) return;
            info.ContextStatistics.NumberOfCommittedTransactions++;
        }

        protected static void UpdateNumberOfConnections(StatisticsBase info)
        {
            if (info == null) return;
            info.ContextStatistics.NumberOfConnections++;
        }

        protected static void UpdateNumberOfExceptions(StatisticsBase info)
        {
            if (info == null) return;
            info.ContextStatistics.NumberOfExceptions++;
        }

        protected static void UpdateNumberOfQueries(StatisticsBase info)
        {
            if (info == null) return;
            info.ContextStatistics.NumberOfQueries++;
        }

        protected static void UpdateNumberOfRolledBackTransactions(StatisticsBase info)
        {
            if (info == null) return;
            info.ContextStatistics.NumberOfRolledBackTransactions++;
        }

        protected static void UpdateNumberOfTransactions(StatisticsBase info)
        {
            if (info == null) return;
            info.ContextStatistics.NumberOfTransactions++;
        }

        protected static void UpdateTotalConnectionOpenTime(StatisticsBase info, CommandConnection connection)
        {
            if (info == null || connection.ClosedAt == null) return;
            info.ContextStatistics.TotalConnectionOpenTime +=
                (long)(connection.ClosedAt.Value - connection.AtDateTime).TotalMilliseconds;
        }

        protected static void UpdateTotalNumberOfRowsReturned(CommandResult item, StatisticsBase info)
        {
            if (info == null || item.RowsReturned == null) return;
            info.ContextStatistics.TotalNumberOfRowsReturned += item.RowsReturned.Value;
        }

        protected static void UpdateTotalQueryExecutionTime(CommandResult item, StatisticsBase info)
        {
            if (info == null || item.ElapsedMilliseconds == null) return;
            info.ContextStatistics.TotalQueryExecutionTime += item.ElapsedMilliseconds.Value;
        }

        protected void ActivateFirstAppIdentity()
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
            {
                GuiModelData.SelectedApplicationIdentity = PluginContext.ProfilerData.ApplicationIdentities.First();
            }
        }

        protected void ActivateRelatedCommands()
        {
            if (GuiModelData.SelectedApplicationIdentity == null)
                return;

            if (GuiModelData.RelatedConnections.Any())
            {
                GuiModelData.SelectedConnection = GuiModelData.RelatedConnections.FirstOrDefault(
                    x => x.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity));
            }
        }
        protected Context GetContext(BaseInfo item)
        {
            return PluginContext.ProfilerData.Contexts
                .FirstOrDefault(x => x.ObjectContextId == item.ObjectContextId &&
                                     x.ApplicationIdentity.Equals(item.ApplicationIdentity));
        }

        protected int GetNumberOfQueriesWithSameHash(Command item)
        {
            return PluginContext.ProfilerData.Commands.Count(
                command => command.ObjectContextId == item.ObjectContextId &&
                     command.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                     command.SqlHash.Equals(item.SqlHash, StringComparison.OrdinalIgnoreCase));
        }

        protected CallingMethodStackTrace GetStackTrace(BaseInfo item)
        {
            return PluginContext.ProfilerData.StackTraces
                .FirstOrDefault(
                    x => x.StackTraceHash.Equals(item.StackTrace.StackTraceHash, StringComparison.OrdinalIgnoreCase) &&
                         x.ApplicationIdentity.Equals(item.ApplicationIdentity));
        }

        protected TrafficUrl GetTrafficUrl(BaseInfo item)
        {
            return string.IsNullOrWhiteSpace(item.HttpInfo.Url)
                ? null
                : PluginContext.ProfilerData.TrafficUrls
                    .FirstOrDefault(
                        x => x.UrlHash.Equals(item.HttpInfo.UrlHash, StringComparison.OrdinalIgnoreCase) &&
                             x.ApplicationIdentity.Equals(item.ApplicationIdentity));
        }

        protected CommandTransaction GetTransaction(BaseInfo item)
        {
            return PluginContext.ProfilerData.Transactions
                .FirstOrDefault(
                    x => x.TransactionId == item.TransactionId &&
                         x.ApplicationIdentity.Equals(item.ApplicationIdentity));
        }

        protected bool HasThisContextDuplicateQueriesWithSameHash(Command item)
        {
            return PluginContext.ProfilerData.Commands.Any(
                command => command.ObjectContextId == item.ObjectContextId &&
                     command.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                     command.CommandId != item.CommandId &&
                     (command.SqlHash.Equals(item.SqlHash, StringComparison.OrdinalIgnoreCase) ||
                     command.NormalizedSqlHash.Equals(item.NormalizedSqlHash, StringComparison.OrdinalIgnoreCase)));
        }

        protected bool HasThisMethodDuplicateQueriesWithSameHash(Command item)
        {
            return PluginContext.ProfilerData.Commands.Any(
                command => command.ObjectContextId == item.ObjectContextId &&
                    command.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                     command.StackTrace.StackTraceHash.Equals(item.StackTrace.StackTraceHash, StringComparison.OrdinalIgnoreCase) &&
                     command.CommandId != item.CommandId &&
                     (command.SqlHash.Equals(item.SqlHash, StringComparison.OrdinalIgnoreCase) ||
                     command.NormalizedSqlHash.Equals(item.NormalizedSqlHash, StringComparison.OrdinalIgnoreCase)));
        }

        protected void ShowSelectedConnectionRelatedCommands(CommandConnection connection)
        {
            if (connection == null || GuiModelData.SelectedApplicationIdentity == null)
                return;

            GuiModelData.RelatedCommands.Clear();
            GuiModelData.RelatedStackTraces.Clear();

            if (!connection.CommandsIds.Any())
                return;

            var commands = PluginContext.ProfilerData.Commands
                .Where(command => command.ConnectionId == connection.ConnectionId &&
                                  command.ApplicationIdentity.Equals(GuiModelData.SelectedApplicationIdentity) &&
                                  command.ObjectContextId == connection.ObjectContextId &&
                                  command.TransactionId == connection.TransactionId &&
                                  command.AtDateTime >= connection.AtDateTime &&
                                  connection.CommandsIds.Contains(command.CommandId ?? 0))
                .OrderBy(command => command.AtDateTime)
                .ToList();

            foreach (var item in commands)
            {
                GuiModelData.RelatedCommands.Add(item);
            }

            ActivateRelatedStackTraces();
        }

        protected void UpdateMultipleContextPerRequestInfo(Context item)
        {
            if (item.HttpContextCurrentId == null)
                return;

            var contexts = PluginContext.ProfilerData.Contexts.Where(
                                x => x.HttpContextCurrentId != null &&
                                     x.ApplicationIdentity.Equals(item.ApplicationIdentity) &&
                                     x.HttpContextCurrentId.Value == item.HttpContextCurrentId.Value)
                                                               .ToList();
            if (contexts.Count < 2)
                return;

            var trafficWebRequest = PluginContext.ProfilerData.TrafficWebRequests
                .FirstOrDefault(x => x.HttpContextCurrentId == item.HttpContextCurrentId.Value &&
                                     x.ApplicationIdentity.Equals(item.ApplicationIdentity));

            if (trafficWebRequest == null)
            {
                var webRequest = new TrafficWebRequest
                {
                    HttpContextCurrentId = item.HttpContextCurrentId.Value,
                    ApplicationIdentity = item.ApplicationIdentity
                };
                foreach (var context in contexts)
                {
                    webRequest.Contexts.Add(context);
                }
                PluginContext.ProfilerData.TrafficWebRequests.Add(webRequest);
            }
            else
            {
                foreach (var context in contexts.Where(context =>
                        !trafficWebRequest.Contexts.Any(
                            x => context.HttpContextCurrentId != null &&
                                 x.HttpContextCurrentId != null &&
                                 x.HttpContextCurrentId.Value == context.HttpContextCurrentId.Value)))
                {
                    trafficWebRequest.Contexts.Add(context);
                }
            }
        }

        protected void UpdateNumberOfDuplicateQueries(Context context, Command item)
        {
            if (HasThisContextDuplicateQueriesWithSameHash(item))
            {
                context.ContextStatistics.NumberOfDuplicateQueries++;
            }
        }
    }
}