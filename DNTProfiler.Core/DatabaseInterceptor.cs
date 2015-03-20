using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using DNTProfiler.Common.Profiler;

namespace DNTProfiler.EntityFramework.Core
{
    public class DatabaseInterceptor : IDbCommandInterceptor, IDbConnectionInterceptor, IDbTransactionInterceptor
    {
        private readonly IDbProfiler _profiler;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public DatabaseInterceptor(IDbProfiler profiler)
        {
            _profiler = profiler;
        }

        public void BeganTransaction(DbConnection connection,
                                     BeginTransactionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbConnection(connection, interceptionContext);
            _profiler.TransactionBegan(connection, context);
        }

        public void BeginningTransaction(DbConnection connection,
                                         BeginTransactionInterceptionContext interceptionContext)
        {
        }

        public void Closed(DbConnection connection,
                           DbConnectionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbConnection(connection, interceptionContext);
            _profiler.ConnectionClosed(connection, context);
        }

        public void Closing(DbConnection connection,
                            DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void Committed(DbTransaction transaction,
                              DbTransactionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbTransaction(transaction, interceptionContext);
            _profiler.TransactionCommitted(transaction, context);
        }

        public void Committing(DbTransaction transaction,
                               DbTransactionInterceptionContext interceptionContext)
        {
        }

        public void ConnectionGetting(DbTransaction transaction,
                                      DbTransactionInterceptionContext<DbConnection> interceptionContext)
        {
        }

        public void ConnectionGot(DbTransaction transaction,
                                  DbTransactionInterceptionContext<DbConnection> interceptionContext)
        {
        }

        public void ConnectionStringGetting(DbConnection connection,
                                            DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionStringGot(DbConnection connection,
                                        DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionStringSet(DbConnection connection,
                                        DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionStringSetting(DbConnection connection,
                                            DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
        }

        public void ConnectionTimeoutGetting(DbConnection connection,
                                             DbConnectionInterceptionContext<int> interceptionContext)
        {
        }

        public void ConnectionTimeoutGot(DbConnection connection,
                                         DbConnectionInterceptionContext<int> interceptionContext)
        {
        }

        public void DatabaseGetting(DbConnection connection,
                                    DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void DatabaseGot(DbConnection connection,
                                DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void DataSourceGetting(DbConnection connection,
                                      DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void DataSourceGot(DbConnection connection,
                                  DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void Disposed(DbConnection connection,
                             DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void Disposed(DbTransaction transaction,
                             DbTransactionInterceptionContext interceptionContext)
        {
        }

        public void Disposing(DbConnection connection,
                              DbConnectionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbConnection(connection, interceptionContext);
            _profiler.ConnectionDisposing(connection, context);
        }

        public void Disposing(DbTransaction transaction,
                              DbTransactionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbTransaction(transaction, interceptionContext);
            _profiler.TransactionDisposing(transaction, context);
        }

        public void EnlistedTransaction(DbConnection connection,
                                        EnlistTransactionInterceptionContext interceptionContext)
        {
        }

        public void EnlistingTransaction(DbConnection connection,
                                         EnlistTransactionInterceptionContext interceptionContext)
        {
        }

        public void IsolationLevelGetting(DbTransaction transaction,
                                          DbTransactionInterceptionContext<IsolationLevel> interceptionContext)
        {
        }

        public void IsolationLevelGot(DbTransaction transaction,
                                      DbTransactionInterceptionContext<IsolationLevel> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command,
                                     DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Stop();
            var context = EFProfilerContextProvider.GetLoggedResult(command, interceptionContext, _stopwatch.ElapsedMilliseconds, null);
            _profiler.NonQueryExecuted(command, context);
        }

        public void NonQueryExecuting(DbCommand command,
                                      DbCommandInterceptionContext<int> interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbCommand(command, interceptionContext);
            _profiler.NonQueryExecuting(command, context);
            _stopwatch.Restart();
        }

        public void Opened(DbConnection connection,
                           DbConnectionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbConnection(connection, interceptionContext);
            _profiler.ConnectionOpened(connection, context);
        }

        public void Opening(DbConnection connection,
                            DbConnectionInterceptionContext interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command,
                                   DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Stop();

            DataTable dataTable = null;

            if (interceptionContext.OriginalException == null)
            {
                var dataSet = new DataSet { EnforceConstraints = false };
                dataTable = new DataTable(tableName: Guid.NewGuid().ToString());
                dataSet.Tables.Add(dataTable);
                dataSet.Load(interceptionContext.OriginalResult, LoadOption.OverwriteChanges, dataTable);

                interceptionContext.Result = dataTable.CreateDataReader();
            }

            var context = EFProfilerContextProvider.GetLoggedResult(command, interceptionContext, _stopwatch.ElapsedMilliseconds, dataTable);
            _profiler.ReaderExecuted(command, context);
        }

        public void ReaderExecuting(DbCommand command,
                                    DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbCommand(command, interceptionContext);
            _profiler.ReaderExecuting(command, context);
            _stopwatch.Restart();
        }

        public void RolledBack(DbTransaction transaction,
                               DbTransactionInterceptionContext interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbTransaction(transaction, interceptionContext);
            _profiler.TransactionRolledBack(transaction, context);
        }

        public void RollingBack(DbTransaction transaction,
                                DbTransactionInterceptionContext interceptionContext)
        {
        }

        public void ScalarExecuted(DbCommand command,
                                   DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Stop();
            var context = EFProfilerContextProvider.GetLoggedResult(command, interceptionContext, _stopwatch.ElapsedMilliseconds, null);
            _profiler.ScalarExecuted(command, context);
        }

        public void ScalarExecuting(DbCommand command,
                                    DbCommandInterceptionContext<object> interceptionContext)
        {
            var context = EFProfilerContextProvider.GetLoggedDbCommand(command, interceptionContext);
            _profiler.ScalarExecuting(command, context);
            _stopwatch.Restart();
        }

        public void ServerVersionGetting(DbConnection connection,
                                         DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void ServerVersionGot(DbConnection connection,
                                     DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        public void StateGetting(DbConnection connection,
                                 DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
        }

        public void StateGot(DbConnection connection,
                             DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
        }
    }
}