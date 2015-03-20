using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Dumper.Models
{
    public class DumperSettings : BaseViewModel
    {
        private string _dumperDirectory;
        private bool _isActive = true;

        public string DumperDirectory
        {
            get { return _dumperDirectory; }
            set
            {
                _dumperDirectory = value;
                NotifyPropertyChanged(() => DumperDirectory);
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                NotifyPropertyChanged(() => IsActive);
            }
        }
    }
}