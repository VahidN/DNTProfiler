using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class UnparametrizedWhereClausesVisitor : SqlFragmentVisitorBase
    {
        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            if (node.SecondExpression.FirstNoneParenthesisExpression() is Literal ||
                node.FirstExpression.FirstNoneParenthesisExpression() is Literal)
            {
                IsSuspected = true;
            }

            base.ExplicitVisit(node);
        }
    }
}