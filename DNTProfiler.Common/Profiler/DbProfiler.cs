using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler.Common.Profiler
{
    public class DbProfiler : IDbProfiler
    {
        private readonly IInfoQueue<BaseInfo> _baseInfoQueue;
        public DbProfiler(IInfoQueue<BaseInfo> baseInfoQueue)
        {
            _baseInfoQueue = baseInfoQueue;
        }

        public SortedSet<string> AssembliesToExclude { set; get; }

        public void ConnectionClosed(DbConnection connection, DbConnectionContext context)
        {
            LogConnection(connection, context, CommandConnectionType.Closed);
        }

        public void ConnectionDisposing(DbConnection connection, DbConnectionContext context)
        {
            LogConnection(connection, context, CommandConnectionType.Disposed);
        }

        public void ConnectionOpened(DbConnection connection, DbConnectionContext context)
        {
            LogConnection(connection, context, CommandConnectionType.Opened);
        }

        public void LogCommand(DbCommand command, DbCommandContext interceptionCommandContext)
        {
            var commandText = command.CommandText ?? "<null>";
            var commandInfo = new Command
            {
                IsAsync = interceptionCommandContext.IsAsync,
                Sql = commandText.Trim(),
                SqlHash = commandText.Trim().ComputeHash(),
                StackTrace = new CallingMethod { AssembliesToExclude = AssembliesToExclude }.GetCallingMethodInfo()
            };
            setBaseInfo(interceptionCommandContext, commandInfo);
            commandInfo.CommandId = UniqueIdExtensions<DbCommand>.GetUniqueId(command).ToInt();

            if (commandInfo.ConnectionId == null)
            {
                commandInfo.ConnectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(command.Connection).ToInt();
            }

            if (command.Transaction != null)
            {
                commandInfo.TransactionId = UniqueIdExtensions<DbTransaction>.GetUniqueId(command.Transaction).ToInt();
                commandInfo.IsolationLevel = command.Transaction.IsolationLevel;
            }

            foreach (var parameter in command.Parameters.OfType<DbParameter>())
            {
                commandInfo.Parameters.Add(logParameter(parameter));
            }

            _baseInfoQueue.Enqueue(commandInfo);
        }

        public void LogConnection(DbConnection connection,
                                         DbConnectionContext interceptionConnectionContext,
                                         CommandConnectionType type)
        {
            var commandConnection = new CommandConnection
            {
                IsAsync = interceptionConnectionContext.IsAsync,
                Type = type,
                IsCanceled = interceptionConnectionContext.IsCanceled,
                ConnectionString = connection.ConnectionString,
                Exception = interceptionConnectionContext.Exception != null ? interceptionConnectionContext.Exception.Message : "",
                StackTrace = new CallingMethod { AssembliesToExclude = AssembliesToExclude }.GetCallingMethodInfo()
            };
            setBaseInfo(interceptionConnectionContext, commandConnection);

            if (commandConnection.ConnectionId == null)
            {
                commandConnection.ConnectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(connection).ToInt();
            }

            _baseInfoQueue.Enqueue(commandConnection);
        }

        public void LogResult(DbCommand command, DbCommandContext interceptionCommandContext)
        {
            var result = interceptionCommandContext.Result;
            var resultString = (object)result == null
                ? "null"
                : (result is DbDataReader)
                    ? result.GetType().Name
                    : result.ToString();

            var commandResult = new CommandResult
            {
                Exception = interceptionCommandContext.Exception != null ? interceptionCommandContext.Exception.Message : "",
                IsCanceled = interceptionCommandContext.IsCanceled,
                ElapsedMilliseconds = interceptionCommandContext.ElapsedMilliseconds,
                ResultString = resultString,
                StackTrace = new CallingMethod { AssembliesToExclude = AssembliesToExclude }.GetCallingMethodInfo(),
                FieldsCount = gefFieldsCount(result)
            };
            setBaseInfo(interceptionCommandContext, commandResult);

            commandResult.CommandId = UniqueIdExtensions<DbCommand>.GetUniqueId(command).ToInt();

            if (commandResult.ConnectionId == null)
            {
                commandResult.ConnectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(command.Connection).ToInt();
            }

            if (command.Transaction != null)
            {
                commandResult.TransactionId = UniqueIdExtensions<DbTransaction>.GetUniqueId(command.Transaction).ToInt();
            }

            if (interceptionCommandContext.DataTable != null && interceptionCommandContext.DataTable.Rows != null)
            {
                commandResult.RowsReturned = interceptionCommandContext.DataTable.Rows.Count;
            }

            _baseInfoQueue.Enqueue(commandResult);
        }


        public void LogTransaction(DbTransaction transaction,
                                   DbTransactionContext interceptionTransactionContext,
                                   CommandTransactionType type)
        {
            var commandTransaction = new CommandTransaction
            {
                IsAsync = interceptionTransactionContext.IsAsync,
                TransactionType = type,
                IsCanceled = interceptionTransactionContext.IsCanceled,
                Exception = interceptionTransactionContext.Exception != null ? interceptionTransactionContext.Exception.Message : "",
                StackTrace = new CallingMethod { AssembliesToExclude = AssembliesToExclude }.GetCallingMethodInfo()
            };
            setBaseInfo(interceptionTransactionContext, commandTransaction);
            commandTransaction.TransactionId = UniqueIdExtensions<DbTransaction>.GetUniqueId(transaction).ToInt();

            _baseInfoQueue.Enqueue(commandTransaction);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandContext context)
        {
            LogResult(command, context);
        }

        public void NonQueryExecuting(DbCommand command, DbCommandContext context)
        {
            LogCommand(command, context);
        }

        public void ReaderExecuted(DbCommand command, DbCommandContext context)
        {
            LogResult(command, context);
        }

        public void ReaderExecuting(DbCommand command, DbCommandContext context)
        {
            LogCommand(command, context);
        }

        public void ScalarExecuted(DbCommand command, DbCommandContext context)
        {
            LogResult(command, context);
        }

        public void ScalarExecuting(DbCommand command, DbCommandContext context)
        {
            LogCommand(command, context);
        }

        public void TransactionBegan(DbConnection connection, DbConnectionContext context)
        {
            var commandTransaction = new CommandTransaction
            {
                IsAsync = context.IsAsync,
                TransactionType = CommandTransactionType.Began,
                IsCanceled = context.IsCanceled,
                Exception = context.Exception != null ? context.Exception.Message : "",
                StackTrace = new CallingMethod { AssembliesToExclude = AssembliesToExclude }.GetCallingMethodInfo()
            };
            setBaseInfo(context, commandTransaction);

            commandTransaction.TransactionId = context.TransactionId;
            commandTransaction.IsolationLevel = context.IsolationLevel;

            _baseInfoQueue.Enqueue(commandTransaction);
        }

        public void TransactionCommitted(DbTransaction transaction, DbTransactionContext transactionContext)
        {
            LogTransaction(transaction, transactionContext, CommandTransactionType.Committed);
        }

        public void TransactionDisposing(DbTransaction transaction, DbTransactionContext transactionContext)
        {
            LogTransaction(transaction, transactionContext, CommandTransactionType.Disposed);
        }

        public void TransactionRolledBack(DbTransaction transaction, DbTransactionContext transactionContext)
        {
            LogTransaction(transaction, transactionContext, CommandTransactionType.RolledBack);
        }

        private static int? gefFieldsCount<TResult>(TResult result)
        {
            if ((object)result == null)
                return null;

            var dbReader = result as DbDataReader;
            if (dbReader == null)
                return null;

            return dbReader.FieldCount;
        }

        private static CommandParameter logParameter(IDbDataParameter parameter)
        {
            return new CommandParameter
            {
                Name = parameter.ParameterName.StartsWith("@") ? parameter.ParameterName : "@" + parameter.ParameterName,
                Value = (parameter.Value == null || parameter.Value == DBNull.Value) ? "null" : parameter.Value.ToString(),
                Type = parameter.DbType.ToString(),
                Direction = parameter.Direction.ToString(),
                IsNullable = parameter.IsNullable,
                Size = parameter.Size,
                Precision = parameter.Precision,
                Scale = parameter.Scale
            };
        }

        private static void setBaseInfo(DbContextBase interceptionContext, BaseInfo info)
        {
            info.ObjectContextId = interceptionContext.ObjectContextId;
            info.ObjectContextName = interceptionContext.ObjectContextName;
            info.ConnectionId = interceptionContext.ConnectionId;
        }
    }
}