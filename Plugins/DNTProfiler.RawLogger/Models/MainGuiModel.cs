using System.Collections.ObjectModel;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.Models;

namespace DNTProfiler.RawLogger.Models
{
    public class MainGuiModel : GuiModelBase
    {
        public MainGuiModel()
        {
            ProfilerItems = new ObservableCollection<BaseInfo>();
        }

        public ObservableCollection<BaseInfo> ProfilerItems { set; get; }

        private BaseInfo _selectedProfilerItem;
        public BaseInfo SelectedProfilerItem
        {
            get { return _selectedProfilerItem; }
            set
            {
                _selectedProfilerItem = value;
                NotifyPropertyChanged(() => SelectedProfilerItem);
            }
        }
    }
}