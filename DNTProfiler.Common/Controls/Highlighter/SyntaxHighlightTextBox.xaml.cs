using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DNTProfiler.Common.Controls.Highlighter
{
    public partial class SyntaxHighlightTextBox
    {
        public static readonly DependencyProperty IsLineNumbersMarginVisibleProperty = DependencyProperty.Register(
            "IsLineNumbersMarginVisible", typeof(bool), typeof(SyntaxHighlightTextBox), new PropertyMetadata(true));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(SyntaxHighlightTextBox), new PropertyMetadata(false, isReadOnlyChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SyntaxHighlightTextBox), new PropertyMetadata("", textChanged));

        private readonly List<InnerTextBlock> _blocks;

        private double _blockHeight;

        private double _lineHeight;

        private DrawingControl _lineNumbersCanvas;

        private Line _lineNumbersSeparator;

        private int _maxLineCountInBlock;

        private DrawingControl _renderCanvas;

        private int _totalLineCount;

        public SyntaxHighlightTextBox()
        {
            InitializeComponent();

            MaxLineCountInBlock = 100;
            LineHeight = FontSize * 1.3;
            _totalLineCount = 1;
            _blocks = new List<InnerTextBlock>();

            this.TextBox1.Loaded += (s, e) =>
             {
                 _renderCanvas = (DrawingControl)TextBox1.Template.FindName("PART_RenderCanvas", this.TextBox1);
                 _lineNumbersCanvas = (DrawingControl)TextBox1.Template.FindName("PART_LineNumbersCanvas", this.TextBox1);
                 var scrollViewer = (ScrollViewer)TextBox1.Template.FindName("PART_ContentHost", this.TextBox1);
                 _lineNumbersSeparator = (Line)TextBox1.Template.FindName("lineNumbersSeparator", this.TextBox1);

                 if (_lineNumbersCanvas != null)
                     _lineNumbersCanvas.Width = GetFormattedTextWidth(string.Format("{0:0000}", _totalLineCount)) + 5;

                 scrollViewer.ScrollChanged += onScrollChanged;

                 invalidateBlocks(0);
                 InvalidateVisual();
             };

            SizeChanged += (s, e) =>
            {
                if (e.HeightChanged == false)
                    return;
                updateBlocks();
                InvalidateVisual();
            };

            this.TextBox1.TextChanged += (s, e) =>
            {
                updateTotalLineCount();
                invalidateBlocks(e.Changes.First().Offset);
                InvalidateVisual();
            };
        }

        public string Highlighter { get; set; }

        public bool IsLineNumbersMarginVisible
        {
            get { return (bool)GetValue(IsLineNumbersMarginVisibleProperty); }
            set { SetValue(IsLineNumbersMarginVisibleProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public double LineHeight
        {
            get { return _lineHeight; }
            set
            {
                if (value != _lineHeight)
                {
                    _lineHeight = value;
                    _blockHeight = MaxLineCountInBlock * value;
                    TextBlock.SetLineStackingStrategy(this, LineStackingStrategy.BlockLineHeight);
                    TextBlock.SetLineHeight(this, _lineHeight);
                }
            }
        }

        public int MaxLineCountInBlock
        {
            get { return _maxLineCountInBlock; }
            set
            {
                _maxLineCountInBlock = value > 0 ? value : 0;
                _blockHeight = value * LineHeight;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Returns the index of the first visible text line.
        /// </summary>
        public int GetIndexOfFirstVisibleLine()
        {
            int guessedLine = (int)(this.TextBox1.VerticalOffset / _lineHeight);
            return guessedLine > _totalLineCount ? _totalLineCount : guessedLine;
        }

        /// <summary>
        /// Returns the index of the last visible text line.
        /// </summary>
        public int GetIndexOfLastVisibleLine()
        {
            double height = this.TextBox1.VerticalOffset + this.TextBox1.ViewportHeight;
            int guessedLine = (int)(height / _lineHeight);
            return guessedLine > _totalLineCount - 1 ? _totalLineCount - 1 : guessedLine;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            DrawBlocks();
            base.OnRender(drawingContext);
        }

        private static void isReadOnlyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var box = dependencyObject as SyntaxHighlightTextBox;
            if (box != null)
            {
                box.TextBox1.IsReadOnly = (bool)e.NewValue;
            }
        }

        private static void textChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var box = dependencyObject as SyntaxHighlightTextBox;
            if (box != null)
            {
                box.TextBox1.Text = e.NewValue.ToString();
            }
        }
        private void DrawBlocks()
        {
            if (!IsLoaded || _renderCanvas == null || _lineNumbersCanvas == null)
                return;

            if (!IsLineNumbersMarginVisible)
            {
                _lineNumbersCanvas.Visibility = Visibility.Collapsed;
                _lineNumbersSeparator.Visibility = Visibility.Collapsed;
            }

            var dc = _renderCanvas.GetContext();
            var dc2 = _lineNumbersCanvas.GetContext();
            for (int i = 0; i < _blocks.Count; i++)
            {
                InnerTextBlock block = _blocks[i];
                Point blockPos = block.Position;
                double top = blockPos.Y - this.TextBox1.VerticalOffset;
                double bottom = top + _blockHeight;
                if (top < ActualHeight && bottom > 0)
                {

                    dc.DrawText(block.FormattedText, new Point(2 - this.TextBox1.HorizontalOffset, block.Position.Y - this.TextBox1.VerticalOffset));
                    if (IsLineNumbersMarginVisible)
                    {
                        _lineNumbersCanvas.Width = GetFormattedTextWidth(string.Format("{0:0000}", _totalLineCount)) + 5;
                        dc2.DrawText(block.LineNumbers, new Point(_lineNumbersCanvas.ActualWidth, 1 + block.Position.Y - this.TextBox1.VerticalOffset));
                    }
                }
            }
            dc.Close();
            dc2.Close();
        }

        /// <summary>
        /// Formats and Highlights the text of a block.
        /// </summary>
        private void formatBlock(InnerTextBlock currentBlock, InnerTextBlock previousBlock)
        {
            currentBlock.FormattedText = GetFormattedText(currentBlock.RawText);
            var previousCode = previousBlock != null ? previousBlock.Code : -1;
            currentBlock.Code = getHighlighter().Highlight(currentBlock.FormattedText, previousCode);
        }

        /// <summary>
        /// Returns a string containing a list of numbers separated with newlines.
        /// </summary>
        private FormattedText getFormattedLineNumbers(int firstIndex, int lastIndex)
        {
            string text = "";
            for (int i = firstIndex + 1; i <= lastIndex + 1; i++)
                text += i + "\n";
            text = text.Trim();

            var ft = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                new SolidColorBrush(Color.FromRgb(0x21, 0xA1, 0xD8)))
            {
                Trimming = TextTrimming.None,
                LineHeight = _lineHeight,
                TextAlignment = TextAlignment.Right
            };

            return ft;
        }

        /// <summary>
        /// Returns a formatted text object from the given string
        /// </summary>
        private FormattedText GetFormattedText(string text)
        {
            var ft = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Black) { Trimming = TextTrimming.None, LineHeight = _lineHeight };

            return ft;
        }

        /// <summary>
        /// Returns the width of a text once formatted.
        /// </summary>
        private double GetFormattedTextWidth(string text)
        {
            var ft = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Black) { Trimming = TextTrimming.None, LineHeight = _lineHeight };

            return ft.Width;
        }

        private IHighlighter getHighlighter()
        {
            if (string.IsNullOrWhiteSpace(Highlighter))
                return HighlighterManager.Highlighters[HighlighterManager.CSharpHighlighter];

            switch (Highlighter.ToLowerInvariant())
            {
                case "sql":
                    return HighlighterManager.Highlighters[HighlighterManager.SqlHighlighter];

                case "c#":
                case "cs":
                case "csharp":
                    return HighlighterManager.Highlighters[HighlighterManager.CSharpHighlighter];

                default:
                    return HighlighterManager.Highlighters[HighlighterManager.SqlHighlighter];
            }
        }

        private void invalidateBlocks(int changeOffset)
        {
            InnerTextBlock blockChanged = null;
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].CharStartIndex <= changeOffset && changeOffset <= _blocks[i].CharEndIndex + 1)
                {
                    blockChanged = _blocks[i];
                    break;
                }
            }

            if (blockChanged == null && changeOffset > 0)
                blockChanged = _blocks.Last();

            int fvline = blockChanged != null ? blockChanged.LineStartIndex : 0;
            int lvline = GetIndexOfLastVisibleLine();
            int fvchar = blockChanged != null ? blockChanged.CharStartIndex : 0;
            int lvchar = TextUtilities.GetLastCharIndexFromLineIndex(this.TextBox1.Text, lvline);

            if (blockChanged != null)
                _blocks.RemoveRange(_blocks.IndexOf(blockChanged), _blocks.Count - _blocks.IndexOf(blockChanged));

            int localLineCount = 1;
            int charStart = fvchar;
            int lineStart = fvline;
            for (int i = fvchar; i < this.TextBox1.Text.Length; i++)
            {
                if (this.TextBox1.Text[i] == '\n')
                {
                    localLineCount += 1;
                }
                if (i == this.TextBox1.Text.Length - 1)
                {
                    string blockText = this.TextBox1.Text.Substring(charStart);
                    var block = new InnerTextBlock(
                        charStart,
                        i, lineStart,
                        lineStart + TextUtilities.GetLineCount(blockText) - 1,
                        LineHeight);
                    block.RawText = block.GetSubString(this.TextBox1.Text);
                    block.LineNumbers = getFormattedLineNumbers(block.LineStartIndex, block.LineEndIndex);
                    block.IsLast = true;

                    foreach (InnerTextBlock b in _blocks)
                        if (b.LineStartIndex == block.LineStartIndex)
                            throw new Exception();

                    _blocks.Add(block);
                    formatBlock(block, _blocks.Count > 1 ? _blocks[_blocks.Count - 2] : null);
                    break;
                }
                if (localLineCount > _maxLineCountInBlock)
                {
                    var block = new InnerTextBlock(
                        charStart,
                        i,
                        lineStart,
                        lineStart + _maxLineCountInBlock - 1,
                        LineHeight);
                    block.RawText = block.GetSubString(this.TextBox1.Text);
                    block.LineNumbers = getFormattedLineNumbers(block.LineStartIndex, block.LineEndIndex);

                    foreach (InnerTextBlock b in _blocks)
                        if (b.LineStartIndex == block.LineStartIndex)
                            throw new Exception();

                    _blocks.Add(block);
                    formatBlock(block, _blocks.Count > 1 ? _blocks[_blocks.Count - 2] : null);

                    charStart = i + 1;
                    lineStart += _maxLineCountInBlock;
                    localLineCount = 1;

                    if (i > lvchar)
                        break;
                }
            }
        }

        private void onScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
                updateBlocks();
            InvalidateVisual();
        }
        private void updateBlocks()
        {
            if (_blocks.Count == 0)
                return;

            // While something is visible after last block...
            while (!_blocks.Last().IsLast && _blocks.Last().Position.Y + _blockHeight - this.TextBox1.VerticalOffset < ActualHeight)
            {
                int firstLineIndex = _blocks.Last().LineEndIndex + 1;
                int lastLineIndex = firstLineIndex + _maxLineCountInBlock - 1;
                lastLineIndex = lastLineIndex <= _totalLineCount - 1 ? lastLineIndex : _totalLineCount - 1;

                int charStart = _blocks.Last().CharEndIndex + 1;
                int lastCharIndex = TextUtilities.GetLastCharIndexFromLineIndex(this.TextBox1.Text, lastLineIndex); // to be optimized (forward search)

                if (lastCharIndex <= charStart)
                {
                    _blocks.Last().IsLast = true;
                    return;
                }

                var block = new InnerTextBlock(
                    charStart,
                    lastCharIndex,
                    _blocks.Last().LineEndIndex + 1,
                    lastLineIndex,
                    LineHeight);
                block.RawText = block.GetSubString(this.TextBox1.Text);
                block.LineNumbers = getFormattedLineNumbers(block.LineStartIndex, block.LineEndIndex);
                _blocks.Add(block);
                formatBlock(block, _blocks.Count > 1 ? _blocks[_blocks.Count - 2] : null);
            }
        }

        private void updateTotalLineCount()
        {
            _totalLineCount = TextUtilities.GetLineCount(this.TextBox1.Text);
        }
    }
}