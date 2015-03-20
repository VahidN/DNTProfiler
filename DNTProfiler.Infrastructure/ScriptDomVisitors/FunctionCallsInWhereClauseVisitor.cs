using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class FunctionCallsInWhereClauseVisitor : SqlFragmentVisitorBase
    {
        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            var secondFunction = node.SecondExpression as FunctionCall;
            checkIsSuspected(secondFunction);

            var firstFunction = node.FirstExpression as FunctionCall;
            checkIsSuspected(firstFunction);

            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(ParenthesisExpression node)
        {
            var function = node.Expression as FunctionCall;
            checkIsSuspected(function);

            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(BooleanIsNullExpression node)
        {
            var function = node.Expression as FunctionCall;
            checkIsSuspected(function);
            base.ExplicitVisit(node);
        }

        private void checkIsSuspected(FunctionCall function)
        {
            if (function == null || function.Parameters == null) return;
            if (function.Parameters.OfType<ColumnReferenceExpression>().Any())
            {
                IsSuspected = true;
            }
        }
    }
}