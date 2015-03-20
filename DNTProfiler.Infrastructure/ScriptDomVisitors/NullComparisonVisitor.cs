using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class NullComparisonVisitor : SqlFragmentVisitorBase
    {
        private readonly IList<string> _declaredNullVariables = new List<string>();

        public IList<string> NullVariableNames { set; get; }

        public NullComparisonVisitor()
        {
            NullVariableNames = new List<string>();
        }

        public override void ExplicitVisit(ExecuteParameter node)
        {
            if (node.ParameterValue is NullLiteral)
            {
                _declaredNullVariables.Add(node.Variable.Name);
            }
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            checkForNullLiterals(node);
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(BooleanParenthesisExpression node)
        {
            var comparisonExpression = node.Expression as BooleanComparisonExpression;
            checkForNullLiterals(comparisonExpression);
            base.ExplicitVisit(node);
        }

        private void checkForNullLiterals(BooleanComparisonExpression comparisonExpression)
        {
            if (comparisonExpression == null) return;

            var firstExpression = comparisonExpression.FirstExpression.FirstNoneParenthesisExpression();
            var secondExpression = comparisonExpression.SecondExpression.FirstNoneParenthesisExpression();
            checkHasNullLiteral(firstExpression, secondExpression, comparisonExpression.ComparisonType);
            checkHasNullVariableReference(firstExpression, secondExpression, comparisonExpression.ComparisonType);
        }

        private void checkComparisonType(BooleanComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case BooleanComparisonType.Equals:
                case BooleanComparisonType.NotEqualToBrackets:
                case BooleanComparisonType.NotEqualToExclamation:
                    IsSuspected = true;
                    break;
            }
        }

        private void checkHasNullLiteral(
            ScalarExpression firstExpression,
            ScalarExpression secondExpression,
            BooleanComparisonType comparisonType)
        {
            if (firstExpression is NullLiteral ||
               secondExpression is NullLiteral)
            {
                checkComparisonType(comparisonType);
            }
        }

        private void checkHasNullVariableReference(
            ScalarExpression firstExpression,
            ScalarExpression secondExpression,
            BooleanComparisonType comparisonType)
        {
            checkHasNullVariableReference(firstExpression, comparisonType);
            checkHasNullVariableReference(secondExpression, comparisonType);
        }

        private void checkHasNullVariableReference(ScalarExpression expression, BooleanComparisonType comparisonType)
        {
            var reference = expression as VariableReference;
            if (reference == null)
                return;

            var parameterName = reference.Name;
            if (!_declaredNullVariables.Contains(parameterName) && !NullVariableNames.Contains(parameterName))
                return;

            checkComparisonType(comparisonType);
        }
    }
}