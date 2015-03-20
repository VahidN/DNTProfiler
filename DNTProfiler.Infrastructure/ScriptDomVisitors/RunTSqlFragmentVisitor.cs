using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class VisitorBaseResult
    {
        public string Name { set; get; }
        public bool Result { set; get; }
    }

    public static class RunTSqlFragmentVisitor
    {
        private static readonly ConcurrentDictionary<string, List<VisitorBaseResult>> _sqlFragmentVisitorsCache =
            new ConcurrentDictionary<string, List<VisitorBaseResult>>();

        public static void AnalyzeFragmentVisitor(string tSql, string sqlHash, TSqlFragmentVisitor visitor)
        {
            var sqlFragment = SqlFragmentProvider.GetSqlFragment(tSql, sqlHash);
            sqlFragment.Accept(visitor);
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
    }
}