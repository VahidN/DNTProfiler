using System.Xml.Linq;

namespace DNTProfiler.Common.Controls.Highlighter
{
    /// <summary>
    /// A regex and its RuleOptions.
    /// </summary>
    public class AdvancedHighlightRule
    {
        public string Expression { get; private set; }
        public RuleOptions Options { get; private set; }

        public AdvancedHighlightRule(XElement rule)
        {
            Expression = rule.Element("Expression").Value.Trim();
            Options = new RuleOptions(rule);
        }
    }
}