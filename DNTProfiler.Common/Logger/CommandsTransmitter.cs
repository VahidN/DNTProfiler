using System;
using System.Net;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Threading;
using DNTProfiler.Common.WebToolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DNTProfiler.Common.Logger
{
    public class CommandsTransmitter : IDisposable
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IInfoQueue<BaseInfo> _infoQueue;
        private readonly string _logFilePath;
        private readonly Uri _serverUri;
        private readonly ISimpleHttp _simpleHttp;

        public CommandsTransmitter(IInfoQueue<BaseInfo> infoQueue,
                                   IExceptionLogger exceptionLogger,
                                   ISimpleHttp simpleHttp,
                                   Uri serverUri,
                                   string logFilePath)
        {
            _infoQueue = infoQueue;
            _logFilePath = logFilePath;
            _exceptionLogger = exceptionLogger;
            _simpleHttp = simpleHttp;
            _serverUri = serverUri;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            _infoQueue.OnNext = result =>
            {
                try
                {
                    postDataAsJson(result);
                }
                catch (Exception ex)
                {
                    _exceptionLogger.LogExceptionToFile(ex, _logFilePath);
                }
            };
        }

        public void Stop()
        {
            _infoQueue.Stop();
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();
        }

        private Uri getRequestUri(BaseInfo result)
        {
            if (result is Command)
            {
                return getUri("/api/Command");
            }

            if (result is CommandResult)
            {
                return getUri("/api/CommandResult");
            }

            if (result is CommandConnection)
            {
                return getUri("/api/CommandConnection");
            }

            if (result is CommandTransaction)
            {
                return getUri("/api/CommandTransaction");
            }

            return null;
        }

        private Uri getUri(string path)
        {
            return new UriBuilder(_serverUri) { Path = path }.Uri;
        }

        private void postDataAsJson(BaseInfo result)
        {
            var response = _simpleHttp.PostAsJson(getRequestUri(result), result,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    Converters = { new StringEnumConverter() },
                    TypeNameHandling = TypeNameHandling.Objects,
                    ContractResolver = new OrderedContractResolver()
                });
            if (response == HttpStatusCode.Created)
            {
                // good!
            }
        }
    }
}