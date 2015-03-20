using System.Collections.Generic;
using System.Linq;
using DNTProfiler.Common.Toolkit;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class FindEqualityJoinVisitor : SqlFragmentVisitorBase
    {
        private readonly Dictionary<string, string> _aliases;
        public FindEqualityJoinVisitor(Dictionary<string, string> aliases)
        {
            EqualityJoins = new List<EqualityJoin>();
            _aliases = aliases;
        }

        public List<EqualityJoin> EqualityJoins { get; private set; }

        public override bool IsSuspected
        {
            get
            {
                return EqualityJoins.Select(x => x.Join)
                                    .GroupBy(join => join)
                                    .Any(group => group.Count() > 1);
            }
        }

        public static FindEqualityJoinVisitor Run(string sql)
        {
            var aliasFinder = new AliasResolutionVisitor();
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitor(sql, sql.ComputeHash(), aliasFinder);

            var visitor = new FindEqualityJoinVisitor(aliasFinder.Aliases);
            RunTSqlFragmentVisitor.AnalyzeFragmentVisitorBase(sql, sql.ComputeHash(), visitor);
            return visitor;
        }

        public override void Visit(QualifiedJoin qualifiedJoin)
        {
            var findEqualityComparisonVisitor = new FindEqualityComparisonVisitor();
            qualifiedJoin.SearchCondition.Accept(findEqualityComparisonVisitor);
            foreach (var equalityComparison in findEqualityComparisonVisitor.Comparisons)
            {
                var firstColumnReferenceExpression = equalityComparison.FirstExpression as ColumnReferenceExpression;
                var secondColumnReferenceExpression = equalityComparison.SecondExpression as ColumnReferenceExpression;
                if (firstColumnReferenceExpression != null && secondColumnReferenceExpression != null)
                {
                    var firstColumnResolved = resolveMultiPartIdentifier(firstColumnReferenceExpression.MultiPartIdentifier);
                    var secondColumnResolved = resolveMultiPartIdentifier(secondColumnReferenceExpression.MultiPartIdentifier);
                    EqualityJoins.Add(new EqualityJoin(firstColumnResolved, secondColumnResolved));
                }
            }
        }

        private string resolveMultiPartIdentifier(MultiPartIdentifier identifier)
        {
            if (identifier.Identifiers.Count == 2 &&
              _aliases.ContainsKey(identifier.Identifiers[0].Value))
            {
                return
                  _aliases[identifier.Identifiers[0].Value] + "." + identifier.Identifiers[1].Value;
            }
            return identifier.AsObjectName();
        }
    }
}