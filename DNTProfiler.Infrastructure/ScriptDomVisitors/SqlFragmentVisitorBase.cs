using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public abstract class SqlFragmentVisitorBase : TSqlFragmentVisitor
    {
        public virtual bool IsSuspected { set; get; }
    }
}