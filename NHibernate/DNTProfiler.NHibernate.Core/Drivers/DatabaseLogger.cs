using System;
using System.Collections.Generic;
using System.Threading;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Profiler;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.Toolkit;
using DNTProfiler.Common.WebToolkit;

namespace DNTProfiler.NHibernate.Core.Drivers
{
    public class DatabaseLogger
    {
        private static readonly Lazy<DatabaseLogger> _instance =
            new Lazy<DatabaseLogger>(() => new DatabaseLogger(), LazyThreadSafetyMode.ExecutionAndPublication);

        private DatabaseLogger()
        {
            AppDomain.MonitoringIsEnabled = true;
            var logger = new ExceptionLogger();
            var baseInfoQueue = new InfoQueue<BaseInfo>();
            Profiler = new DbProfiler(baseInfoQueue)
            {
                AssembliesToExclude = new SortedSet<string>
                {
                    typeof(DbProfiler).Assembly.GetName().Name,
                    typeof(DatabaseLogger).Assembly.GetName().Name
                }
            };
            var serverUri = new Uri(ConfigSetGet.GetConfigData("DNTProfilerServerUri"));
            var logFilePath = ConfigSetGet.GetConfigData("DNTProfilerLogFilePath");
            var commandsTransmitter = new CommandsTransmitter(
                baseInfoQueue, logger, new SimpleHttp(), serverUri, LoggerPath.GetLogFileFullPath(logFilePath));
            commandsTransmitter.Start();
        }

        public static DatabaseLogger Instance
        {
            get { return _instance.Value; }
        }

        public DbProfiler Profiler { get; private set; }
    }
}