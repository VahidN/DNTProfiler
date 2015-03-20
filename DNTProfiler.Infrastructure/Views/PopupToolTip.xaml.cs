using System.Windows;

namespace DNTProfiler.Infrastructure.Views
{
    public partial class PopupToolTip
    {
        public static readonly DependencyProperty TextProperty =
               DependencyProperty.Register("Text",
                                           typeof(string),
                                           typeof(PopupToolTip),
                                           new PropertyMetadata(string.Empty));

        public PopupToolTip()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}