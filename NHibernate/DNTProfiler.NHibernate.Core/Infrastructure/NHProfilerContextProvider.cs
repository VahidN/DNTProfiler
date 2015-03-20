using System;
using System.Data;
using System.Data.Common;
using DNTProfiler.Common.Profiler;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.Toolkit;
using NHibernate.Impl;

namespace DNTProfiler.NHibernate.Core.Infrastructure
{
    public static class NHProfilerContextProvider
    {
        public static DbCommandContext GetLoggedDbCommand(DbCommand command, Exception exception)
        {
            var context = new DbCommandContext
            {
                IsAsync = false,
                IsCanceled = false,
                Exception = exception,
                ObjectContextId = SessionIdLoggingContext.SessionId.GetUniqueId(),
                ObjectContextName = SessionId,
                ConnectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(command.Connection).ToInt()
            };
            return context;
        }

        public static DbConnectionContext GetLoggedDbConnection(ProfiledDbTransaction interceptionContext, int connectionId)
        {
            var context = new DbConnectionContext
            {
                IsAsync = false,
                IsCanceled = false,
                Exception = null,
                ConnectionString = interceptionContext.Connection.ConnectionString,
                TransactionId =
                    interceptionContext.InnerTransaction == null
                        ? (int?)null
                        : UniqueIdExtensions<DbTransaction>.GetUniqueId(interceptionContext.InnerTransaction).ToInt(),
                IsolationLevel = interceptionContext.IsolationLevel,
                ObjectContextId = SessionIdLoggingContext.SessionId.GetUniqueId(),
                ObjectContextName = SessionId,
                ConnectionId = connectionId
            };
            return context;
        }

        public static DbConnectionContext GetLoggedDbConnection(DbConnection connection)
        {
            var context = new DbConnectionContext
            {
                IsAsync = false,
                IsCanceled = false,
                Exception = null,
                ConnectionString = connection.ConnectionString,
                ObjectContextId = SessionIdLoggingContext.SessionId.GetUniqueId(),
                ObjectContextName = SessionId,
                ConnectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(connection).ToInt()
            };
            return context;
        }

        public static DbTransactionContext GetLoggedDbTransaction(DbTransaction transaction, int connectionId)
        {
            var context = new DbTransactionContext
            {
                IsAsync = false,
                IsCanceled = false,
                Exception = null,
                ObjectContextId = SessionIdLoggingContext.SessionId.GetUniqueId(),
                ObjectContextName = SessionId,
                ConnectionId = connectionId,
                TransactionId = UniqueIdExtensions<DbTransaction>.GetUniqueId(transaction).ToInt()
            };
            return context;
        }

        public static DbCommandContext GetLoggedResult(
                                DbCommand command, Exception exception, object result,
                                long? elapsedMilliseconds, DataTable dataTable)
        {
            var context = new DbCommandContext
            {
                IsAsync = false,
                IsCanceled = false,
                Exception = exception,
                Result = result,
                ElapsedMilliseconds = elapsedMilliseconds,
                DataTable = dataTable,
                ObjectContextId = SessionIdLoggingContext.SessionId.GetUniqueId(),
                ObjectContextName = SessionId,
                ConnectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(command.Connection).ToInt()
            };
            return context;
        }

        public static string SessionId
        {
            get { return SessionIdLoggingContext.SessionId == null ? "Default" : SessionIdLoggingContext.SessionId.ToString(); }
        }
    }
}