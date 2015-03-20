using System.Windows;
using System.Windows.Media;

namespace DNTProfiler.Common.Controls.Highlighter
{
    public class InnerTextBlock
    {
        public string RawText { get; set; }
        public FormattedText FormattedText { get; set; }
        public FormattedText LineNumbers { get; set; }
        public int CharStartIndex { get; private set; }
        public int CharEndIndex { get; private set; }
        public int LineStartIndex { get; private set; }
        public int LineEndIndex { get; private set; }
        public Point Position { get { return new Point(0, LineStartIndex * _lineHeight); } }
        public bool IsLast { get; set; }
        public int Code { get; set; }

        private readonly double _lineHeight;

        public InnerTextBlock(int charStart, int charEnd, int lineStart, int lineEnd, double lineHeight)
        {
            CharStartIndex = charStart;
            CharEndIndex = charEnd;
            LineStartIndex = lineStart;
            LineEndIndex = lineEnd;
            this._lineHeight = lineHeight;
            IsLast = false;

        }

        public string GetSubString(string text)
        {
            return text.Substring(CharStartIndex, CharEndIndex - CharStartIndex + 1);
        }

        public override string ToString()
        {
            return string.Format("L:{0}/{1} C:{2}/{3} {4}",
                LineStartIndex,
                LineEndIndex,
                CharStartIndex,
                CharEndIndex,
                FormattedText.Text);
        }
    }
}