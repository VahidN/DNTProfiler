using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Common.Models
{
    public class CommandStatistics : BaseViewModel
    {
        private int _deletesCount;
        private int _insertsCount;
        private int _joinsCount;
        private int _likesCount;
        private int _selectsCount;
        private int _updatesCount;

        public int DeletesCount
        {
            get { return _deletesCount; }
            set
            {
                _deletesCount = value;
                NotifyPropertyChanged(() => DeletesCount);
            }
        }

        public int InsertsCount
        {
            get { return _insertsCount; }
            set
            {
                _insertsCount = value;
                NotifyPropertyChanged(() => InsertsCount);
            }
        }

        public int JoinsCount
        {
            get { return _joinsCount; }
            set
            {
                _joinsCount = value;
                NotifyPropertyChanged(() => JoinsCount);
            }
        }

        public int LikesCount
        {
            get { return _likesCount; }
            set
            {
                _likesCount = value;
                NotifyPropertyChanged(() => LikesCount);
            }
        }

        public int SelectsCount
        {
            get { return _selectsCount; }
            set
            {
                _selectsCount = value;
                NotifyPropertyChanged(() => SelectsCount);
            }
        }

        public int UpdatesCount
        {
            get { return _updatesCount; }
            set
            {
                _updatesCount = value;
                NotifyPropertyChanged(() => UpdatesCount);
            }
        }
    }
}