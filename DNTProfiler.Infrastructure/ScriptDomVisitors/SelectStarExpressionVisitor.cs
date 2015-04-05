using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class SelectStarExpressionVisitor : SqlFragmentVisitorBase
    {
        public override void ExplicitVisit(SelectStarExpression node)
        {
            IsSuspected = true;
            base.ExplicitVisit(node);
        }
    }
}