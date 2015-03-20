using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class EqualityJoin
    {
        private readonly SchemaObjectName _left;
        private readonly SchemaObjectName _right;

        public EqualityJoin(string qualifiedObjectNameLeft, string qualifiedObjectNameRight)
        {
            var parser = new TSql120Parser(initialQuotedIdentifiers: false);
            IList<ParseError> errors;
            using (var reader = new StringReader(qualifiedObjectNameLeft))
            {
                _left = parser.ParseSchemaObjectName(reader, out errors);
            }
            using (var reader = new StringReader(qualifiedObjectNameRight))
            {
                _right = parser.ParseSchemaObjectName(reader, out errors);
            }
        }

        public bool JoinsSameDatabase
        {
            get { return _left.Identifiers[0].Value == _right.Identifiers[0].Value; }
        }

        public SchemaObjectName Left { get { return _left; } }

        public string LeftObjectName
        {
            get { return _left.AsObjectName(); }
        }

        public SchemaObjectName Right { get { return _right; } }

        public string RightObjectName
        {
            get { return _right.AsObjectName(); }
        }

        public string Join
        {
            get
            {
                return new[] { LeftObjectName.ToLowerInvariant(), RightObjectName.ToLowerInvariant() }
                    .OrderBy(join => join)
                    .Aggregate((join1, join2) => string.Format("{0} = {1}", join1, join2));
            }
        }

        public override string ToString()
        {
            return Join;
        }
    }
}