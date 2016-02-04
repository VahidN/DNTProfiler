using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class UnParametrizedSkipAndTakeVisitor : SqlFragmentVisitorBase
    {
        public override void ExplicitVisit(OffsetClause node)
        {
            if (node.OffsetExpression.FirstNoneParenthesisExpression() is Literal ||
                node.FetchExpression.FirstNoneParenthesisExpression() is Literal)
            {
                IsSuspected = true;
            }

            base.ExplicitVisit(node);
        }
    }
}