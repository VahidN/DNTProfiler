using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class UnparametrizedWhereClausesVisitor : SqlFragmentVisitorBase
    {
        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            var secondExpression = node.SecondExpression.FirstNoneParenthesisExpression();
            var firstExpression = node.FirstExpression.FirstNoneParenthesisExpression();
            checkHasUnparametrizedWhereClauses(secondExpression, firstExpression);

            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(LikePredicate node)
        {
            var secondExpression = node.SecondExpression.FirstNoneParenthesisExpression();
            var firstExpression = node.FirstExpression.FirstNoneParenthesisExpression();
            checkHasUnparametrizedWhereClauses(secondExpression, firstExpression);

            base.ExplicitVisit(node);
        }

        private void checkHasUnparametrizedWhereClauses(ScalarExpression secondExpression, ScalarExpression firstExpression)
        {
            checkIsLiteral(secondExpression, firstExpression);

            checkIsSuspectedFunctionCall(firstExpression);
            checkIsSuspectedFunctionCall(secondExpression);

            checkIsSuspectedCastCall(firstExpression);
            checkIsSuspectedCastCall(secondExpression);
        }

        private void checkIsLiteral(ScalarExpression secondExpression, ScalarExpression firstExpression)
        {
            if (secondExpression is Literal || firstExpression is Literal)
            {
                IsSuspected = true;
            }
        }

        private void checkIsSuspectedCastCall(ScalarExpression scalarExpression)
        {
            var castCall = scalarExpression as CastCall;
            if (castCall == null || castCall.Parameter == null) return;

            if (castCall.Parameter is Literal)
            {
                IsSuspected = true;
            }
        }

        private void checkIsSuspectedFunctionCall(ScalarExpression scalarExpression)
        {
            var function = scalarExpression as FunctionCall;
            if (function == null || function.Parameters == null) return;

            if (function.Parameters.OfType<Literal>().Any())
            {
                IsSuspected = true;
            }
        }
    }
}