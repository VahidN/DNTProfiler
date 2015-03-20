using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class FindEqualityComparisonVisitor : TSqlFragmentVisitor
    {
        public FindEqualityComparisonVisitor()
        {
            Comparisons = new List<BooleanComparisonExpression>();
        }

        public List<BooleanComparisonExpression> Comparisons { get; private set; }

        public override void Visit(BooleanComparisonExpression expression)
        {
            if (expression.IsEqualityComparison())
            {
                Comparisons.Add(expression);
            }
        }
    }
}