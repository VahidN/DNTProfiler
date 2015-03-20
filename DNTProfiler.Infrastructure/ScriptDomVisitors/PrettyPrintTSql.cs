using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public static class PrettyPrintTSql
    {
        private static readonly Dictionary<string, string> _formattedSql = new Dictionary<string, string>();

        public static string FormatTSql(this string tSql, string sqlHash)
        {
            if (string.IsNullOrWhiteSpace(tSql))
                return string.Empty;

            string formattedTSql;
            if (_formattedSql.TryGetValue(sqlHash, out formattedTSql))
            {
                return formattedTSql;
            }

            var sqlFragment = SqlFragmentProvider.GetSqlFragment(tSql, sqlHash);
            var scriptGenerator = new Sql120ScriptGenerator(new SqlScriptGeneratorOptions
            {
                SqlVersion = SqlVersion.Sql120,
                KeywordCasing = KeywordCasing.Uppercase
            });

            scriptGenerator.GenerateScript(sqlFragment, out  formattedTSql);
            formattedTSql = !string.IsNullOrWhiteSpace(formattedTSql) ? formattedTSql.Trim() : tSql;

            _formattedSql.Add(sqlHash, formattedTSql);
            return formattedTSql;
        }
    }
}