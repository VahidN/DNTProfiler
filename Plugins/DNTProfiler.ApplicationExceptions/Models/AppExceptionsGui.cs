using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.ApplicationExceptions.Models
{
    public class AppExceptionsGui : BaseViewModel
    {
        private AppException _selectedAppException;

        public AppExceptionsGui()
        {
            AppExceptionsList = new AppExceptionsList();
        }

        public AppExceptionsList AppExceptionsList { set; get; }

        public AppException SelectedAppException
        {
            get { return _selectedAppException; }
            set
            {
                _selectedAppException = value;
                NotifyPropertyChanged(() => SelectedAppException);
            }
        }
    }
}