using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public static class SqlFragmentProvider
    {
        private static readonly ConcurrentDictionary<string, TSqlScript> _sqlFragments =
            new ConcurrentDictionary<string, TSqlScript>();

        public static TSqlScript GetSqlFragment(string tSql, string sqlHash, int timeoutSeconds = 7, bool readFromCache = true)
        {
            var runner = TimedRunner.RunWithTimeout(() =>
            {
                try
                {
                    return getSqlFragment(tSql, sqlHash, readFromCache);
                }
                catch (Exception ex)
                {
                    new ExceptionLogger().LogExceptionToFile(ex, AppMessenger.LogFile);
                    DispatcherHelper.DispatchAction(() => AppMessenger.Messenger.NotifyColleagues("ShowException", ex));
                    return null;
                }
            }, timeoutSeconds);

            if (runner.IsTimedOut)
            {
                throw new TimeoutException(string.Format("Timed out parsing SqlFragment:{0}{1}",
                    Environment.NewLine, tSql));
            }

            return runner.Result;
        }

        private static TSqlScript getSqlFragment(string tSql, string sqlHash, bool readFromCache)
        {
            TSqlScript sqlFragment;
            if (readFromCache && _sqlFragments.TryGetValue(sqlHash, out sqlFragment))
            {
                return sqlFragment;
            }

            IList<ParseError> errors;
            using (var reader = new StringReader(tSql))
            {
                var parser = new TSql120Parser(initialQuotedIdentifiers: true);
                sqlFragment = (TSqlScript)parser.Parse(reader, out errors);
            }

            if (errors == null || !errors.Any())
            {
                if (!readFromCache)
                {
                    _sqlFragments.TryAdd(sqlHash, sqlFragment);
                }
                return sqlFragment;
            }

            var sb = new StringBuilder();
            sb.AppendLine(tSql);
            sb.AppendLine("Errors:");
            foreach (var error in errors)
                sb.AppendLine(error.Message);

            throw new InvalidOperationException(sb.ToString());
        }
    }
}