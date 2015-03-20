using System;
using System.Data;
using System.Data.Common;
using DNTProfiler.Common.Profiler;
using NHibernate.Impl;

namespace DNTProfiler.NHibernate.Core.Infrastructure
{
    [System.ComponentModel.DesignerCategory("")]
    public class ProfiledDbConnection : DbConnection, ICloneable
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly IDbProfiler _profiler;

        public ProfiledDbConnection(DbConnection connection, IDbProfiler profiler)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            InnerConnection = connection;
            InnerConnection.StateChange += StateChangeHandler;

            if (profiler != null)
            {
                _profiler = profiler;
            }
        }

        public override string ConnectionString
        {
            get { return InnerConnection.ConnectionString; }
            set { InnerConnection.ConnectionString = value; }
        }

        public override int ConnectionTimeout
        {
            get { return InnerConnection.ConnectionTimeout; }
        }

        public override string Database
        {
            get { return InnerConnection.Database; }
        }

        public override string DataSource
        {
            get { return InnerConnection.DataSource; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public DbConnection InnerConnection { get; private set; }

        public override string ServerVersion
        {
            get { return InnerConnection.ServerVersion; }
        }

        public Guid? SessionId
        {
            get { return SessionIdLoggingContext.SessionId; }
        }
        public override ConnectionState State
        {
            get { return InnerConnection.State; }
        }

        protected override bool CanRaiseEvents
        {
            get { return true; }
        }

        public override void ChangeDatabase(string databaseName)
        {
            InnerConnection.ChangeDatabase(databaseName);
        }

        public ProfiledDbConnection Clone()
        {
            var tail = InnerConnection as ICloneable;
            if (tail == null) throw new NotSupportedException(string.Format("Underlying {0} is not cloneable", InnerConnection.GetType().Name));
            return new ProfiledDbConnection((DbConnection)tail.Clone(), _profiler);
        }

        public override void Close()
        {
            _profiler.ConnectionClosed(InnerConnection, NHProfilerContextProvider.GetLoggedDbConnection(InnerConnection));
            InnerConnection.Close();
        }

        public override void EnlistTransaction(System.Transactions.Transaction transaction)
        {
            InnerConnection.EnlistTransaction(transaction);
        }

        public override DataTable GetSchema()
        {
            return InnerConnection.GetSchema();
        }

        public override DataTable GetSchema(string collectionName)
        {
            return InnerConnection.GetSchema(collectionName);
        }

        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            return InnerConnection.GetSchema(collectionName, restrictionValues);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public override void Open()
        {
            _profiler.ConnectionOpened(InnerConnection, NHProfilerContextProvider.GetLoggedDbConnection(InnerConnection));
            InnerConnection.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new ProfiledDbTransaction(InnerConnection.BeginTransaction(isolationLevel), this, _profiler);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new ProfiledDbCommand(InnerConnection.CreateCommand(), this, _profiler);
        }

        protected override void Dispose(bool disposing)
        {
            _profiler.ConnectionDisposing(InnerConnection, NHProfilerContextProvider.GetLoggedDbConnection(InnerConnection));
            if (disposing && InnerConnection != null)
            {
                InnerConnection.StateChange -= StateChangeHandler;
                InnerConnection.Dispose();
            }
            InnerConnection = null;
            base.Dispose(disposing);
        }

        private void StateChangeHandler(object sender, StateChangeEventArgs stateChangeEventArguments)
        {
            OnStateChange(stateChangeEventArguments);
        }
    }
}