using System;
using System.Data;
using System.Data.Common;
using DNTProfiler.Common.Profiler;
using DNTProfiler.NHibernate.Core.Infrastructure;
using NHibernate.AdoNet;
using NHibernate.Driver;

namespace DNTProfiler.NHibernate.Core.Drivers
{
    public class ProfiledSql2008ClientDriver : Sql2008ClientDriver, IEmbeddedBatcherFactoryProvider
    {
        private readonly IDbProfiler _profiler;

        public ProfiledSql2008ClientDriver(IDbProfiler profiler)
        {
            _profiler = profiler;
        }

        public ProfiledSql2008ClientDriver()
            : this(DatabaseLogger.Instance.Profiler)
        { }

        Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
        {
            get { return typeof(ProfiledSqlClientBatchingBatcherFactory); }
        }

        public override IDbCommand CreateCommand()
        {
            var command = base.CreateCommand();
            return new ProfiledDbCommand((DbCommand)command, null, _profiler);
        }

        public override IDbConnection CreateConnection()
        {
            var connection = base.CreateConnection();
            return new ProfiledDbConnection((DbConnection)connection, _profiler);
        }
    }
}