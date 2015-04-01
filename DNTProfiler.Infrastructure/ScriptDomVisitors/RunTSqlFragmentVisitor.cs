using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DNTProfiler.Common.Toolkit;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public static class RunTSqlFragmentVisitor
    {
        private static readonly ConcurrentDictionary<string, NormalizedSqlResult> _normalizedSqlHashCache =
            new ConcurrentDictionary<string, NormalizedSqlResult>();

        private static readonly ConcurrentDictionary<string, List<VisitorBaseResult>> _sqlFragmentVisitorsCache =
            new ConcurrentDictionary<string, List<VisitorBaseResult>>();

        public static TSqlScript AnalyzeFragmentVisitor(string tSql, string sqlHash, TSqlFragmentVisitor visitor)
        {
            var sqlFragment = SqlFragmentProvider.GetSqlFragment(tSql, sqlHash);
            if (sqlFragment == null)
            {
                throw new InvalidOperationException(string.Format("Failed to get sqlFragment of \n{0}", tSql));
            }

            sqlFragment.Accept(visitor);
            return sqlFragment;
        }

        public static bool AnalyzeFragmentVisitorBase(string tSql, string sqlHash, SqlFragmentVisitorBase visitor)
        {
            var typeName = visitor.GetType().Name;
            List<VisitorBaseResult> result;
            if (_sqlFragmentVisitorsCache.TryGetValue(sqlHash, out result))
            {
                var visitorBaseResult = result.FirstOrDefault(x => x.Name == typeName);
                if (visitorBaseResult != null)
                {
                    return visitorBaseResult.Result;
                }
            }

            AnalyzeFragmentVisitor(tSql, sqlHash, visitor);

            var baseResult = new VisitorBaseResult
            {
                Name = typeName,
                Result = visitor.IsSuspected
            };

            if (result != null)
            {
                result.Add(baseResult);
                _sqlFragmentVisitorsCache[sqlHash] = result;
            }
            else
            {
                _sqlFragmentVisitorsCache.TryAdd(sqlHash, new List<VisitorBaseResult> { baseResult });
            }

            return visitor.IsSuspected;
        }

        public static NormalizedSqlResult GetNormalizedSqlHash(string sql, string sqlHash)
        {
            NormalizedSqlResult result;
            if (_normalizedSqlHashCache.TryGetValue(sqlHash, out result))
            {
                return result;
            }

            var scriptFragment = AnalyzeFragmentVisitor(sql, sqlHash, new SqlNormalizerVisitor());
            var scriptGenerator = new Sql120ScriptGenerator();
            string script;
            scriptGenerator.GenerateScript(scriptFragment, out script);

            result = new NormalizedSqlResult
            {
                OriginalSqlHash = sqlHash,
                NormalizedSqlHash = script.Trim().ComputeHash()
            };

            _normalizedSqlHashCache.TryAdd(sqlHash, result);

            return result;
        }
    }

    public class NormalizedSqlResult
    {
        public string NormalizedSqlHash { set; get; }
        public string OriginalSqlHash { set; get; }
    }

    public class VisitorBaseResult
    {
        public string Name { set; get; }
        public bool Result { set; get; }
    }
}