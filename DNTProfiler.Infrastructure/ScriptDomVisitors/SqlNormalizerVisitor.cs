using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class SqlNormalizerVisitor : TSqlFragmentVisitor
    {
        public override void ExplicitVisit(InPredicate node)
        {
            if (node.Values != null)
            {
                var count = node.Values.Count;
                if (count > 1)
                {
                    for (var i = count - 1; i > 0; i--)
                    {
                        if (node.Values[i] is Literal)
                        {
                            node.Values.RemoveAt(i);
                        }
                    }
                }
            }

            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(NumericLiteral node)
        {
            node.Value = "0.1";
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(MoneyLiteral node)
        {
            node.Value = "$1";
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(BinaryLiteral node)
        {
            node.Value = "0xABCD";
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(RealLiteral node)
        {
            node.Value = "0.1E-2";
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(IntegerLiteral node)
        {
            node.Value = "0";
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(StringLiteral node)
        {
            node.Value = "test";
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(VariableReference node)
        {
            node.Name = "@p1";
            base.ExplicitVisit(node);
        }
    }
}