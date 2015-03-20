using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace DNTProfiler.Common.Behaviors
{
    public static class DelayedUpdateBehavior
    {
        #region TargetProperty Attached DependencyProperty
        /// <summary>
        /// An Attached <see cref="DependencyProperty"/> of type <see cref="DependencyProperty"/> defined on <see cref="DependencyObject">DependencyObject instances</see>.
        /// </summary>
        public static readonly DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached(
          TargetPropertyPropertyName,
          typeof(DependencyProperty),
          typeof(DelayedUpdateBehavior),
          new FrameworkPropertyMetadata(null, onTargetPropertyChanged)
        );

        /// <summary>
        /// The name of the <see cref="TargetPropertyProperty"/> Attached <see cref="DependencyProperty"/>.
        /// </summary>
        public const string TargetPropertyPropertyName = "TargetProperty";

        /// <summary>
        /// Sets the value of the <see cref="TargetPropertyProperty"/> on the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject">target element</see>.</param>
        /// <param name="value"></param>
        public static void SetTargetProperty(DependencyObject element, DependencyProperty value)
        {
            element.SetValue(TargetPropertyProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="TargetPropertyProperty"/> as set on the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject">target element</see>.</param>
        /// <returns><see cref="DependencyProperty"/></returns>
        public static DependencyProperty GetTargetProperty(DependencyObject element)
        {
            return (DependencyProperty)element.GetValue(TargetPropertyProperty);
        }

        /// <summary>
        /// Called when <see cref="TargetPropertyProperty"/> changes
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject">event source</see>.</param>
        /// <param name="e"><see cref="DependencyPropertyChangedEventArgs">event arguments</see></param>
        private static void onTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var prop = e.NewValue as DependencyProperty;
            if (prop == null)
                return;
            d.Dispatcher.BeginInvoke(
                (Action<DependencyObject, DependencyProperty>)
                    ((target, p) => new PropertyChangeTimer(target, p)),
                DispatcherPriority.ApplicationIdle,
                d,
                prop);

        }
        #endregion
        #region Milliseconds Attached DependencyProperty
        /// <summary>
        /// An Attached <see cref="DependencyProperty"/> of type <see cref="int"/> defined on <see cref="DependencyObject">DependencyObject instances</see>.
        /// </summary>
        public static readonly DependencyProperty MillisecondsProperty = DependencyProperty.RegisterAttached(
          MillisecondsPropertyName,
          typeof(int),
          typeof(DelayedUpdateBehavior),
          new FrameworkPropertyMetadata(1000)
        );

        /// <summary>
        /// The name of the <see cref="MillisecondsProperty"/> Attached <see cref="DependencyProperty"/>.
        /// </summary>
        public const string MillisecondsPropertyName = "Milliseconds";

        /// <summary>
        /// Sets the value of the <see cref="MillisecondsProperty"/> on the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject">target element</see>.</param>
        /// <param name="value"></param>
        public static void SetMilliseconds(DependencyObject element, int value)
        {
            element.SetValue(MillisecondsProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="MillisecondsProperty"/> as set on the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The <see cref="DependencyObject">target element</see>.</param>
        /// <returns><see cref="int"/></returns>
        public static int GetMilliseconds(DependencyObject element)
        {
            return (int)element.GetValue(MillisecondsProperty);
        }
        #endregion
        private class PropertyChangeTimer
        {
            private DispatcherTimer _timer;
            private readonly BindingExpression _expression;
            public PropertyChangeTimer(DependencyObject target, DependencyProperty property)
            {
                if (target == null)
                    throw new ArgumentNullException("target");
                if (property == null)
                    throw new ArgumentNullException("property");
                if (!BindingOperations.IsDataBound(target, property))
                    return;
                _expression = BindingOperations.GetBindingExpression(target, property);
                if (_expression == null)
                    throw new InvalidOperationException("No binding was found on property " + property.Name + " on object " + target.GetType().FullName);
                DependencyPropertyDescriptor.FromProperty(property, target.GetType()).AddValueChanged(target, onPropertyChanged);
            }

            private void onPropertyChanged(object sender, EventArgs e)
            {
                if (_timer == null)
                {
                    _timer = new DispatcherTimer();
                    int ms = GetMilliseconds(sender as DependencyObject);
                    _timer.Interval = TimeSpan.FromMilliseconds(ms);
                    _timer.Tick += onTimerTick;
                    _timer.Start();
                    return;
                }
                _timer.Stop();
                _timer.Start();
            }

            private void onTimerTick(object sender, EventArgs e)
            {
                _expression.UpdateSource();
                _expression.UpdateTarget();
                _timer.Stop();
                _timer = null;
            }
        }
    }
}