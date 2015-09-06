using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class UseFirstLevelCacheVisitor : SqlFragmentVisitorBase
    {
        private int _comparisonCount;
        private bool _hasTopRowFilter;

        public ISet<string> Keys { set; get; }

        public override void ExplicitVisit(BooleanComparisonExpression node)
        {
            if (!_hasTopRowFilter)
            {
                IsSuspected = false;
                base.ExplicitVisit(node);
                return;
            }

            _comparisonCount++;
            if (_comparisonCount > 1)
            {
                IsSuspected = false;
                base.ExplicitVisit(node);
                return;
            }

            if (node.ComparisonType != BooleanComparisonType.Equals)
            {
                IsSuspected = false;
                base.ExplicitVisit(node);
                return;
            }

            var secondExpression = node.SecondExpression.FirstNoneParenthesisExpression();
            var firstExpression = node.FirstExpression.FirstNoneParenthesisExpression();

            if (isCastOrFunctionCall(firstExpression) || isCastOrFunctionCall(secondExpression))
            {
                IsSuspected = false;
                base.ExplicitVisit(node);
                return;
            }

            var firstColumnReferenceExpression = firstExpression as ColumnReferenceExpression;
            if (firstColumnReferenceExpression != null)
            {
                var firstColumnResolved = resolveMultiPartIdentifier(firstColumnReferenceExpression.MultiPartIdentifier);
                if (Keys != null && Keys.Contains(firstColumnResolved))
                {
                    IsSuspected = true;
                    _comparisonCount--;
                    base.ExplicitVisit(node);
                    return;
                }

                if (IsSuspected)
                {
                    IsSuspected = false;
                    base.ExplicitVisit(node);
                    return;
                }
            }

            var secondColumnReferenceExpression = secondExpression as ColumnReferenceExpression;
            if (secondColumnReferenceExpression != null)
            {
                var secondColumnResolved = resolveMultiPartIdentifier(secondColumnReferenceExpression.MultiPartIdentifier);
                if (Keys != null && Keys.Contains(secondColumnResolved))
                {
                    IsSuspected = true;
                    _comparisonCount--;
                    base.ExplicitVisit(node);
                    return;
                }

                if (IsSuspected)
                {
                    IsSuspected = false;
                    base.ExplicitVisit(node);
                    return;
                }
            }

            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(TopRowFilter node)
        {
            _hasTopRowFilter = true;
            base.ExplicitVisit(node);
        }

        private static bool isCastOrFunctionCall(ScalarExpression expression)
        {
            return expression is CastCall || expression is FunctionCall;
        }

        private static string resolveMultiPartIdentifier(MultiPartIdentifier identifier)
        {
            return identifier.Identifiers.Count == 2 ? identifier.Identifiers[1].Value : identifier.AsObjectName();
        }
    }
}