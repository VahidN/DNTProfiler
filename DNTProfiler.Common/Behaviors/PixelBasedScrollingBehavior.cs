using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace DNTProfiler.Common.Behaviors
{
    /// <summary>
    /// Smooth scrolling VirtualizingStackPanels, without sacrificing virtualization.
    /// </summary>
    public static class PixelBasedScrollingBehavior
    {
        private static readonly MethodInfo _setScrollUnit = typeof(VirtualizingPanel)
            .GetMethod("SetScrollUnit", BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo _setCacheLengthUnit = typeof(VirtualizingPanel)
            .GetMethod("SetCacheLengthUnit", BindingFlags.Public | BindingFlags.Static);

        private static readonly MethodInfo _setCacheLength = typeof(VirtualizingPanel)
            .GetMethod("SetCacheLength", BindingFlags.Public | BindingFlags.Static);

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PixelBasedScrollingBehavior), new UIPropertyMetadata(false, handleIsEnabledChanged));

        private static void handleIsEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var listView = obj as ListView;
            if (listView == null)
            {
                throw new InvalidOperationException("This behavior can only be attached to a ListView.");
            }

            if (_setScrollUnit != null)
            {
                // It's .NET 4.5
                _setScrollUnit.Invoke(listView, new object[] { listView, /*Pixel*/ 0 });
            }

            if (_setCacheLengthUnit != null)
            {
                // It's .NET 4.5
                _setCacheLengthUnit.Invoke(listView, new object[] { listView, /*Pixel*/ 0 });
            }


            if (_setCacheLength != null)
            {
                // It's .NET 4.5
                var type = Type.GetType("System.Windows.Controls.VirtualizationCacheLength, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                if (type == null) return;
                var instance = Activator.CreateInstance(type, 100.0);

                _setCacheLength.Invoke(listView, new[] { listView, instance });
            }
        }
    }
}