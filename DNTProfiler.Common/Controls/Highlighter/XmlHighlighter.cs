using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml.Linq;

namespace DNTProfiler.Common.Controls.Highlighter
{
    /// <summary>
    /// An IHighlighter built from an Xml syntax file
    /// </summary>
    public class XmlHighlighter : IHighlighter
    {
        private readonly List<HighlightWordsRule> _wordsRules;
        private readonly List<HighlightLineRule> _lineRules;
        private readonly List<AdvancedHighlightRule> _regexRules;

        private static readonly Regex _wordsRegex = new Regex("[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled | RegexOptions.Multiline);

        public XmlHighlighter(XElement root)
        {
            _wordsRules = new List<HighlightWordsRule>();
            _lineRules = new List<HighlightLineRule>();
            _regexRules = new List<AdvancedHighlightRule>();

            foreach (var elem in root.Elements())
            {
                switch (elem.Name.ToString())
                {
                    case "HighlightWordsRule": _wordsRules.Add(new HighlightWordsRule(elem)); break;
                    case "HighlightLineRule": _lineRules.Add(new HighlightLineRule(elem)); break;
                    case "AdvancedHighlightRule": _regexRules.Add(new AdvancedHighlightRule(elem)); break;
                }
            }
        }

        public int Highlight(FormattedText text, int previousBlockCode)
        {
            //
            // WORDS RULES
            //
            foreach (Match match in _wordsRegex.Matches(text.Text))
            {
                foreach (var rule in _wordsRules)
                {
                    foreach (var word in rule.Words.Select(word => word.Trim()))
                    {
                        if (rule.Options.IgnoreCase)
                        {
                            if (match.Value.Trim().Equals(word, StringComparison.InvariantCultureIgnoreCase))
                            {
                                text.SetForegroundBrush(rule.Options.Foreground, match.Index, match.Length);
                                text.SetFontWeight(rule.Options.FontWeight, match.Index, match.Length);
                                text.SetFontStyle(rule.Options.FontStyle, match.Index, match.Length);
                            }
                        }
                        else
                        {
                            if (match.Value.Trim() == word)
                            {
                                text.SetForegroundBrush(rule.Options.Foreground, match.Index, match.Length);
                                text.SetFontWeight(rule.Options.FontWeight, match.Index, match.Length);
                                text.SetFontStyle(rule.Options.FontStyle, match.Index, match.Length);
                            }
                        }
                    }
                }
            }

            //
            // REGEX RULES
            //
            foreach (AdvancedHighlightRule rule in _regexRules)
            {
                Regex regexRgx = new Regex(rule.Expression);
                foreach (Match m in regexRgx.Matches(text.Text))
                {
                    text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                    text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                    text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
                }
            }

            //
            // LINES RULES
            //
            foreach (HighlightLineRule rule in _lineRules)
            {
                Regex lineRgx = new Regex(Regex.Escape(rule.LineStart) + ".*");
                foreach (Match m in lineRgx.Matches(text.Text))
                {
                    text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                    text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                    text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
                }
            }

            return -1;
        }
    }
}