using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler.Common.Controls
{
    public class ScrollableTabControl : TabControl
    {
        private ScrollViewer _tabScrollViewer;
        private Panel _tabPanelTop;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _tabScrollViewer = GetTemplateChild("TabScrollViewerTop") as ScrollViewer;
            _tabPanelTop = GetTemplateChild("HeaderPanel") as Panel;
            SelectionChanged += (s, e) => scrollToSelectedItem();
        }

        private void scrollToSelectedItem()
        {
            if (_tabScrollViewer == null)
                return;

            var model = SelectedItem;
            var tabItem = ItemContainerGenerator.ContainerFromItem(model) as TabItem;
            if (tabItem == null || _tabScrollViewer == null)
                return;

            if (tabItem.ActualWidth.ApproxEquals(0) && !tabItem.IsLoaded)
            {
                tabItem.Loaded += (s, e) => scrollToSelectedItem();
                return;
            }

            scrollToItem(tabItem);
        }

        private void scrollToItem(TabItem selectedTabItem)
        {
            if (_tabScrollViewer == null || _tabPanelTop == null)
                return;

            var tabItems = Items.Cast<object>()
                .Select(item => ItemContainerGenerator.ContainerFromItem(item) as TabItem).ToList();

            var leftTabItems = new List<TabItem>();
            int index;
            for (index = 0; index < tabItems.Count; index++)
            {
                var tabItem = tabItems[index];
                if (!Equals(tabItem, selectedTabItem))
                {
                    leftTabItems.Add(tabItem);
                }
                else
                {
                    break;
                }
            }

            var leftItemsWidth = leftTabItems.Sum(tabItem => tabItem.ActualWidth);

            if (leftItemsWidth + selectedTabItem.ActualWidth > _tabScrollViewer.HorizontalOffset + _tabScrollViewer.ViewportWidth)
            {
                var currentHorizontalOffset = (leftItemsWidth + selectedTabItem.ActualWidth) - _tabScrollViewer.ViewportWidth;
                var hMargin = !leftTabItems.Any(ti => ti.IsSelected) && !selectedTabItem.IsSelected ? _tabPanelTop.Margin.Left + _tabPanelTop.Margin.Right : 0;
                currentHorizontalOffset += hMargin;
                if (index + 1 < tabItems.Count)
                {
                    var nextItem = tabItems[index + 1];
                    currentHorizontalOffset += nextItem.ActualWidth;
                }
                _tabScrollViewer.ScrollToHorizontalOffset(currentHorizontalOffset);
            }
            else if (leftItemsWidth < _tabScrollViewer.HorizontalOffset)
            {
                var currentHorizontalOffset = leftItemsWidth;
                var hMargin = leftTabItems.Any(ti => ti.IsSelected) ? _tabPanelTop.Margin.Left + _tabPanelTop.Margin.Right : 0;
                currentHorizontalOffset -= hMargin;
                if (index - 1 >= 0)
                {
                    var previousItem = tabItems[index - 1];
                    currentHorizontalOffset -= previousItem.ActualWidth;
                }
                _tabScrollViewer.ScrollToHorizontalOffset(currentHorizontalOffset);
            }
        }
    }
}