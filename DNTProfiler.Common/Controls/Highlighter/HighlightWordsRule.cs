using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DNTProfiler.Common.Controls.Highlighter
{
    /// <summary>
    /// A set of words and their RuleOptions.
    /// </summary>
    public class HighlightWordsRule
    {
        public List<string> Words { get; private set; }
        public RuleOptions Options { get; private set; }

        private static readonly Regex _splitRegex =
            new Regex(@"(\s|\(|\)|\+|\-|\%|\*|\[|\]|/)", RegexOptions.Compiled | RegexOptions.Multiline);

        public HighlightWordsRule(XElement rule)
        {
            Words = new List<string>();
            Options = new RuleOptions(rule);

            var wordsStr = rule.Element("Words").Value;
            var words = _splitRegex.Split(wordsStr);

            foreach (var word in words)
                if (!string.IsNullOrWhiteSpace(word))
                    Words.Add(word.Trim());
        }
    }
}