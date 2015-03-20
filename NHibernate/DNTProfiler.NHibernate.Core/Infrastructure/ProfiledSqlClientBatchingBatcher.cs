using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;

namespace DNTProfiler.NHibernate.Core.Infrastructure
{
    public class ProfiledSqlClientBatchingBatcher : AbstractBatcher
    {
        private int _batchSize;
        private int _totalExpectedRowsAffected;
        private SqlClientSqlCommandSet _currentBatch;
        private StringBuilder _currentBatchCommandsLog;
        private readonly int _defaultTimeout;

        public ProfiledSqlClientBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
            : base(connectionManager, interceptor)
        {
            _batchSize = Factory.Settings.AdoBatchSize;
            _defaultTimeout = PropertiesHelper.GetInt32(global::NHibernate.Cfg.Environment.CommandTimeout, global::NHibernate.Cfg.Environment.Properties, -1);

            _currentBatch = createConfiguredBatch();
            _currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
        }

        public override int BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        protected override int CountOfStatementsInCurrentBatch
        {
            get { return _currentBatch.CountOfCommands; }
        }

        public override void AddToBatch(IExpectation expectation)
        {
            _totalExpectedRowsAffected += expectation.ExpectedRowCount;
            var batchUpdate = CurrentCommand;
            Driver.AdjustCommand(batchUpdate);
            var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
            if (sqlStatementLogger.IsDebugEnabled)
            {
                var lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
                var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
                lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
                _currentBatchCommandsLog.Append("command ")
                    .Append(_currentBatch.CountOfCommands)
                    .Append(":")
                    .AppendLine(lineWithParameters);
            }

            var update = batchUpdate as ProfiledDbCommand;
            if (update != null)
            {
                _currentBatch.Append((SqlCommand)update.InternalCommand);
            }
            else
            {
                _currentBatch.Append((SqlCommand)batchUpdate);
            }


            if (_currentBatch.CountOfCommands >= _batchSize)
            {
                ExecuteBatchWithTiming(batchUpdate);
            }
        }

        protected void ProfiledPrepare(IDbCommand cmd)
        {
            try
            {
                var sessionConnection = ConnectionManager.GetConnection();

                if (cmd.Connection != null)
                {
                    // make sure the commands connection is the same as the Sessions connection
                    // these can be different when the session is disconnected and then reconnected
                    if (cmd.Connection != sessionConnection)
                    {
                        cmd.Connection = sessionConnection;
                    }
                }
                else
                {
                    cmd.Connection = (sessionConnection as ProfiledDbConnection).InnerConnection;
                }

                var trans = (ProfiledDbTransaction)typeof(global::NHibernate.Transaction.AdoTransaction).InvokeMember("trans", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, ConnectionManager.Transaction, null);
                if (trans != null)
                    cmd.Transaction = trans.InnerTransaction;
                Factory.ConnectionProvider.Driver.PrepareCommand(cmd);
            }
            catch (InvalidOperationException ioe)
            {
                throw new ADOException("While preparing " + cmd.CommandText + " an error occurred", ioe);
            }
        }

        protected override void DoExecuteBatch(IDbCommand ps)
        {
            CheckReaders();
            ProfiledPrepare(_currentBatch.BatchCommand);
            if (Factory.Settings.SqlStatementLogger.IsDebugEnabled)
            {
                Factory.Settings.SqlStatementLogger.LogBatchCommand(_currentBatchCommandsLog.ToString());
                _currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
            }

            int rowsAffected;
            try
            {
                rowsAffected = _currentBatch.ExecuteNonQuery();
            }
            catch (DbException e)
            {
                throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not execute batch command.");
            }

            Expectations.VerifyOutcomeBatched(_totalExpectedRowsAffected, rowsAffected);

            _currentBatch.Dispose();
            _totalExpectedRowsAffected = 0;
            _currentBatch = createConfiguredBatch();
        }

        private SqlClientSqlCommandSet createConfiguredBatch()
        {
            var result = new SqlClientSqlCommandSet();
            if (_defaultTimeout <= 0)
                return result;

            try
            {
                result.CommandTimeout = _defaultTimeout;
            }
            catch
            { }

            return result;
        }
    }
}