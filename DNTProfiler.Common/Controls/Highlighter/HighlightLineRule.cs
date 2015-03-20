using System.Xml.Linq;

namespace DNTProfiler.Common.Controls.Highlighter
{
    /// <summary>
    /// A line start definition and its RuleOptions.
    /// </summary>
    public class HighlightLineRule
    {
        public string LineStart { get; private set; }
        public RuleOptions Options { get; private set; }

        public HighlightLineRule(XElement rule)
        {
            LineStart = rule.Element("LineStart").Value.Trim();
            Options = new RuleOptions(rule);
        }
    }
}