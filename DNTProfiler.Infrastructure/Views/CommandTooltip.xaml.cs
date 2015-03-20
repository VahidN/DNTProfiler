using System.Windows;
using DNTProfiler.Common.Models;

namespace DNTProfiler.Infrastructure.Views
{
    public partial class CommandTooltip
    {
        public static readonly DependencyProperty CommandProperty =
               DependencyProperty.Register("Command",
                                           typeof(Command),
                                           typeof(CommandTooltip),
                                           new PropertyMetadata(null));

        public CommandTooltip()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public Command Command
        {
            get { return (Command)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}