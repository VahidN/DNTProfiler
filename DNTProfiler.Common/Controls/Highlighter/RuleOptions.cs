using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace DNTProfiler.Common.Controls.Highlighter
{
    /// <summary>
    /// A set of options liked to each rule.
    /// </summary>
    public class RuleOptions
    {
        public bool IgnoreCase { get; private set; }
        public Brush Foreground { get; private set; }
        public FontWeight FontWeight { get; private set; }
        public FontStyle FontStyle { get; private set; }

        public RuleOptions(XElement rule)
        {
            string ignoreCaseStr = rule.Element("IgnoreCase").Value.Trim();
            string foregroundStr = rule.Element("Foreground").Value.Trim();
            string fontWeightStr = rule.Element("FontWeight").Value.Trim();
            string fontStyleStr = rule.Element("FontStyle").Value.Trim();

            IgnoreCase = bool.Parse(ignoreCaseStr);
            var brush = (Brush)new BrushConverter().ConvertFrom(foregroundStr);
            brush.Freeze();
            Foreground = brush;
            FontWeight = (FontWeight)new FontWeightConverter().ConvertFrom(fontWeightStr);
            FontStyle = (FontStyle)new FontStyleConverter().ConvertFrom(fontStyleStr);
        }
    }
}
