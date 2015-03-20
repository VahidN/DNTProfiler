using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class AliasResolutionVisitor : TSqlFragmentVisitor
    {
        public AliasResolutionVisitor()
        {
            Aliases = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Aliases { get; private set; }

        public override void Visit(NamedTableReference namedTableReference)
        {
            var alias = namedTableReference.Alias;
            if (alias != null)
            {
                var baseObjectName = namedTableReference.SchemaObject.AsObjectName();
                Aliases.Add(alias.Value, baseObjectName);
            }
        }
    }
}