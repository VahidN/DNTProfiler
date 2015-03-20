using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Profiler;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.WebToolkit;

namespace DNTProfiler.EntityFramework.Core
{
    public class DatabaseLogger : IDisposable, IDbConfigurationInterceptor
    {
        private readonly string _logFilePath;
        private readonly Uri _serverUri;
        private InfoQueue<BaseInfo> _baseInfoQueue;
        private CommandsTransmitter _commandsTransmitter;

        public DatabaseLogger(string serverUri, string logFilePath)
        {
            _serverUri = new Uri(serverUri);
            _logFilePath = logFilePath;
            AppDomain.MonitoringIsEnabled = true;
        }

        public bool IsFinished
        {
            get { return _baseInfoQueue.IsEmpty; }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Loaded(DbConfigurationLoadedEventArgs loadedEventArgs,
                           DbConfigurationInterceptionContext interceptionContext)
        {
            var logger = new ExceptionLogger();
            _baseInfoQueue = new InfoQueue<BaseInfo>();
            var profiler = new DbProfiler(_baseInfoQueue)
            {
                AssembliesToExclude = new SortedSet<string>
                {
                    typeof(DbProfiler).Assembly.GetName().Name,
                    typeof(DatabaseLogger).Assembly.GetName().Name
                }
            };
            DbInterception.Add(new DatabaseInterceptor(profiler));
            _commandsTransmitter = new CommandsTransmitter(
                _baseInfoQueue, logger, new SimpleHttp(), _serverUri, LoggerPath.GetLogFileFullPath(_logFilePath));
            _commandsTransmitter.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_commandsTransmitter == null) return;
            _commandsTransmitter.Stop();

            if (disposing && _commandsTransmitter != null)
            {
                _commandsTransmitter.Dispose();
                _commandsTransmitter = null;
            }
        }
    }
}