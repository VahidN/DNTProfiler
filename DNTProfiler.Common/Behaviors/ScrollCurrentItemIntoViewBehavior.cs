using System.Windows;
using System.Windows.Controls;

namespace DNTProfiler.Common.Behaviors
{
    public static class ScrollCurrentItemIntoViewBehavior
    {
        public static readonly DependencyProperty AutoScrollToCurrentItemProperty =
            DependencyProperty.RegisterAttached("AutoScrollToCurrentItem",
                typeof(bool), typeof(ScrollCurrentItemIntoViewBehavior),
                new UIPropertyMetadata(default(bool), OnAutoScrollToCurrentItemChanged));

        public static bool GetAutoScrollToCurrentItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToCurrentItemProperty);
        }

        public static void OnAutoScrollToCurrentItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var listBox = obj as ListBox;
            if (listBox == null) return;

            var newValue = (bool)e.NewValue;
            if (newValue)
                listBox.SelectionChanged += listBoxSelectionChanged;
            else
                listBox.SelectionChanged -= listBoxSelectionChanged;
        }

        static void listBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null || listBox.Items == null) return;

            listBox.Items.MoveCurrentTo(listBox.SelectedItem);
            listBox.ScrollIntoView(listBox.SelectedItem);
        }

        public static void SetAutoScrollToCurrentItem(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToCurrentItemProperty, value);
        }
    }
}