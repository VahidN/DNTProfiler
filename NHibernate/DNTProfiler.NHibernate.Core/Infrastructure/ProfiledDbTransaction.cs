using System;
using System.Data;
using System.Data.Common;
using DNTProfiler.Common.Profiler;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.Toolkit;
using NHibernate.Impl;

namespace DNTProfiler.NHibernate.Core.Infrastructure
{
    public class ProfiledDbTransaction : DbTransaction
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly IDbProfiler _profiler;
        private ProfiledDbConnection _connection;
        private readonly int _connectionId;

        public ProfiledDbTransaction(DbTransaction transaction, ProfiledDbConnection connection, IDbProfiler profiler)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (connection == null) throw new ArgumentNullException("connection");
            InnerTransaction = transaction;
            _connection = connection;
            _profiler = profiler;
            _connectionId = UniqueIdExtensions<DbConnection>.GetUniqueId(connection.InnerConnection).ToInt();
            _profiler.TransactionBegan(connection.InnerConnection, NHProfilerContextProvider.GetLoggedDbConnection(this, _connectionId));
        }

        public Guid Id
        {
            get { return _id; }
        }

        public DbTransaction InnerTransaction { get; private set; }

        public override IsolationLevel IsolationLevel
        {
            get { return InnerTransaction.IsolationLevel; }
        }

        public Guid? SessionId
        {
            get { return SessionIdLoggingContext.SessionId; }
        }

        protected override DbConnection DbConnection
        {
            get { return _connection; }
        }

        public override void Commit()
        {
            _profiler.TransactionCommitted(InnerTransaction, NHProfilerContextProvider.GetLoggedDbTransaction(InnerTransaction, _connectionId));
            InnerTransaction.Commit();
        }

        public override void Rollback()
        {
            _profiler.TransactionRolledBack(InnerTransaction, NHProfilerContextProvider.GetLoggedDbTransaction(InnerTransaction, _connectionId));
            InnerTransaction.Rollback();
        }

        protected override void Dispose(bool disposing)
        {
            _profiler.TransactionDisposing(InnerTransaction, NHProfilerContextProvider.GetLoggedDbTransaction(InnerTransaction, _connectionId));
            if (disposing && InnerTransaction != null)
            {
                InnerTransaction.Dispose();
            }
            InnerTransaction = null;
            _connection = null;
            base.Dispose(disposing);
        }
    }
}