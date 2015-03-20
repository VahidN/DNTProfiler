using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DNTProfiler.Common.Mvvm
{
    public class CommandAction : TriggerAction<UIElement>
    {
        public static DependencyProperty CommandProperty = 
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandAction), null);

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public static DependencyProperty ParameterProperty = 
            DependencyProperty.Register("Parameter", typeof(object), typeof(CommandAction), null);

        public object Parameter
        {
            get
            {
                return GetValue(ParameterProperty);
            }
            set
            {
                SetValue(ParameterProperty, value);

            }
        }

        protected override void Invoke(object parameter)
        {
            Command.Execute(Parameter);
        }
    }
}