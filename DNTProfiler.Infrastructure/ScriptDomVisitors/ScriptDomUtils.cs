using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public static class ScriptDomUtils
    {
        public static ScalarExpression FirstNoneParenthesisExpression(this ScalarExpression scalarExpression)
        {
            var parenthesisExpression = scalarExpression as ParenthesisExpression;
            if (parenthesisExpression == null)
            {
                return scalarExpression;
            }

            return FirstNoneParenthesisExpression(parenthesisExpression.Expression);
        }

        public static bool IsEqualityComparison(this BooleanExpression expression)
        {
            return
              expression is BooleanComparisonExpression &&
              ((BooleanComparisonExpression)expression).ComparisonType == BooleanComparisonType.Equals;
        }

        public static string AsObjectName(this MultiPartIdentifier multiPartIdentifier)
        {
            return string.Join(".", multiPartIdentifier.Identifiers.Select(i => i.Value));
        }
    }
}