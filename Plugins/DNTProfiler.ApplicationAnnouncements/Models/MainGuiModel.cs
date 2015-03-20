using System.Collections.Generic;
using DNTProfiler.Infrastructure.Models;

namespace DNTProfiler.ApplicationAnnouncements.Models
{
    public class MainGuiModel : GuiModelBase
    {
        private IList<ReleaseInfo> _releaseInfo;
        private ReleaseInfo _selectedRelease;

        public IList<ReleaseInfo> ReleaseInfo
        {
            get { return _releaseInfo; }
            set
            {
                _releaseInfo = value;
                NotifyPropertyChanged(() => ReleaseInfo);
            }
        }
        public ReleaseInfo SelectedRelease
        {
            get { return _selectedRelease; }
            set
            {
                _selectedRelease = value;
                NotifyPropertyChanged(() => SelectedRelease);
            }
        }
    }
}