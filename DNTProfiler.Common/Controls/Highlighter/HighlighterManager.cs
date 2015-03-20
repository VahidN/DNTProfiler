using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace DNTProfiler.Common.Controls.Highlighter
{
    public static class HighlighterManager
    {
        public const  string SqlHighlighter = "DNTProfiler.Common.Controls.Highlighter.SQL.xml";
        public const string CSharpHighlighter = "DNTProfiler.Common.Controls.Highlighter.CS.xml";

        public static IDictionary<string, IHighlighter> Highlighters { get; private set; }

        private static IHighlighter getHighlightingDefinition(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException("resourceName");

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new NullReferenceException(string.Format("{0} resource is null.", resourceName));

                var xmlDoc = XDocument.Load(stream);
                var root = xmlDoc.Root;
                return new XmlHighlighter(root);
            }
        }

        static HighlighterManager()
        {
            Highlighters = new Dictionary<string, IHighlighter>
			{
			    { SqlHighlighter, getHighlightingDefinition(SqlHighlighter) },
                { CSharpHighlighter, getHighlightingDefinition(CSharpHighlighter) },
			};
        }
    }
}