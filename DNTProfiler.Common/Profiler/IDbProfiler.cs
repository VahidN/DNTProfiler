using System.Data.Common;

namespace DNTProfiler.Common.Profiler
{
    public interface IDbProfiler
    {
        void ConnectionClosed(DbConnection connection, DbConnectionContext context);

        void ConnectionDisposing(DbConnection connection, DbConnectionContext context);

        void ConnectionOpened(DbConnection connection, DbConnectionContext context);

        void NonQueryExecuted(DbCommand command, DbCommandContext context);

        void NonQueryExecuting(DbCommand command, DbCommandContext context);

        void ReaderExecuted(DbCommand command, DbCommandContext context);

        void ReaderExecuting(DbCommand command, DbCommandContext context);

        void ScalarExecuted(DbCommand command, DbCommandContext context);

        void ScalarExecuting(DbCommand command, DbCommandContext context);

        void TransactionBegan(DbConnection connection, DbConnectionContext context);

        void TransactionCommitted(DbTransaction transaction, DbTransactionContext transactionContext);

        void TransactionDisposing(DbTransaction transaction, DbTransactionContext transactionContext);

        void TransactionRolledBack(DbTransaction transaction, DbTransactionContext transactionContext);
    }
}
