using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public static class SqlFragmentProvider
    {
        private static readonly Dictionary<string, TSqlScript> _sqlFragments = new Dictionary<string, TSqlScript>();

        public static TSqlScript GetSqlFragment(string tSql, string sqlHash, int timeoutSeconds = 7)
        {
            var runner = TimedRunner.RunWithTimeout(() => getSqlFragment(tSql, sqlHash), timeoutSeconds);
            if (runner.IsTimedOut)
                throw new TimeoutException(string.Format("Timed out parsing SqlFragment:{0}{1}", Environment.NewLine, tSql));

            return runner.Result;
        }

        private static TSqlScript getSqlFragment(string tSql, string sqlHash)
        {
            TSqlScript sqlFragment;
            if (_sqlFragments.TryGetValue(sqlHash, out sqlFragment))
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
                _sqlFragments.Add(sqlHash, sqlFragment);
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