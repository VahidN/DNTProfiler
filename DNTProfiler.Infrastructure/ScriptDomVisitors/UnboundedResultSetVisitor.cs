using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class UnboundedResultSetVisitor : SqlFragmentVisitorBase
    {
        public UnboundedResultSetVisitor()
        {
            IsSuspected = true;
        }

        public override void ExplicitVisit(WhereClause node)
        {
            IsSuspected = false;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(TopRowFilter node)
        {
            IsSuspected = false;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(OffsetClause node)
        {
            IsSuspected = false;
            base.ExplicitVisit(node);
        }
    }
}