using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using DNTProfiler.Common.Profiler;
using NHibernate.Impl;

namespace DNTProfiler.NHibernate.Core.Infrastructure
{
    [System.ComponentModel.DesignerCategory("")]
    public class ProfiledDbCommand : DbCommand, ICloneable
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly IDbProfiler _profiler;
        private DbConnection _connection;
        private DbTransaction _transaction;

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public ProfiledDbCommand(DbCommand command, DbConnection connection, IDbProfiler profiler)
        {
            if (command == null) throw new ArgumentNullException("command");

            InternalCommand = command;
            _connection = connection;

            _profiler = profiler;
        }

        public int AffectedRows { set; get; }

        public override string CommandText
        {
            get { return InternalCommand.CommandText; }
            set { InternalCommand.CommandText = value; }
        }

        public override int CommandTimeout
        {
            get { return InternalCommand.CommandTimeout; }
            set { InternalCommand.CommandTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return InternalCommand.CommandType; }
            set { InternalCommand.CommandType = value; }
        }

        public DataTable DataRows { get; private set; }

        public override bool DesignTimeVisible
        {
            get { return InternalCommand.DesignTimeVisible; }
            set { InternalCommand.DesignTimeVisible = value; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public DbCommand InternalCommand { get; private set; }

        public Guid? SessionId
        {
            get { return SessionIdLoggingContext.SessionId; }
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get { return InternalCommand.UpdatedRowSource; }
            set { InternalCommand.UpdatedRowSource = value; }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return _connection;
            }

            set
            {
                _connection = value;
                var dbConnection = value as ProfiledDbConnection;
                InternalCommand.Connection = dbConnection == null ? value : dbConnection.InnerConnection;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return InternalCommand.Parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return _transaction;
            }

            set
            {
                _transaction = value;
                var awesomeTran = value as ProfiledDbTransaction;
                InternalCommand.Transaction = awesomeTran == null ? value : awesomeTran.InnerTransaction;
            }
        }

        public override void Cancel()
        {
            InternalCommand.Cancel();
        }

        public ProfiledDbCommand Clone()
        {
            var tail = InternalCommand as ICloneable;
            if (tail == null) throw new NotSupportedException("Underlying " + InternalCommand.GetType().Name + " is not cloneable");
            return new ProfiledDbCommand((DbCommand)tail.Clone(), _connection, _profiler);
        }

        public override int ExecuteNonQuery()
        {
            int result = 0;

            var context = NHProfilerContextProvider.GetLoggedDbCommand(InternalCommand, null);
            _profiler.NonQueryExecuting(InternalCommand, context);
            _stopwatch.Restart();
            try
            {
                result = InternalCommand.ExecuteNonQuery();
                AffectedRows = result;
            }
            catch (Exception e)
            {
                context = NHProfilerContextProvider.GetLoggedDbCommand(InternalCommand, e);
                _profiler.NonQueryExecuting(InternalCommand, context);
                throw;
            }
            finally
            {
                _stopwatch.Stop();
                context = NHProfilerContextProvider.GetLoggedResult(InternalCommand, null, result, _stopwatch.ElapsedMilliseconds, null);
                _profiler.NonQueryExecuted(InternalCommand, context);
            }

            return result;
        }

        public override object ExecuteScalar()
        {
            object result = null;
            var context = NHProfilerContextProvider.GetLoggedDbCommand(InternalCommand, null);
            _profiler.ScalarExecuting(InternalCommand, context);
            _stopwatch.Restart();
            try
            {
                result = InternalCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                context = NHProfilerContextProvider.GetLoggedDbCommand(InternalCommand, e);
                _profiler.ScalarExecuting(InternalCommand, context);
                throw;
            }
            finally
            {
                _stopwatch.Stop();
                context = NHProfilerContextProvider.GetLoggedResult(InternalCommand, null, result, _stopwatch.ElapsedMilliseconds, null);
                _profiler.ScalarExecuted(InternalCommand, context);
            }

            return result;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public override void Prepare()
        {
            InternalCommand.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return InternalCommand.CreateParameter();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && InternalCommand != null)
            {
                InternalCommand.Dispose();
            }
            InternalCommand = null;
            base.Dispose(disposing);
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            DbDataReader originalResult = null;

            var context = NHProfilerContextProvider.GetLoggedDbCommand(InternalCommand, null);
            _profiler.ReaderExecuting(InternalCommand, context);
            _stopwatch.Restart();
            try
            {
                originalResult = InternalCommand.ExecuteReader(behavior);

                var dataSet = new DataSet { EnforceConstraints = false };
                DataRows = new DataTable(tableName: Guid.NewGuid().ToString());
                dataSet.Tables.Add(DataRows);
                dataSet.Load(originalResult, LoadOption.OverwriteChanges, DataRows);

                originalResult = DataRows.CreateDataReader();
            }
            catch (Exception e)
            {
                context = NHProfilerContextProvider.GetLoggedDbCommand(InternalCommand, e);
                _profiler.ReaderExecuting(InternalCommand, context);
                throw;
            }
            finally
            {
                _stopwatch.Stop();
                context = NHProfilerContextProvider.GetLoggedResult(InternalCommand, null, originalResult, _stopwatch.ElapsedMilliseconds, DataRows);
                _profiler.ReaderExecuted(InternalCommand, context);
            }

            return originalResult;
        }
    }
}